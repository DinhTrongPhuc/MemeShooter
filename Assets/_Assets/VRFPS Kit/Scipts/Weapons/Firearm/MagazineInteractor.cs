using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRFPSKit
{
    /// <summary>
    /// Extends SocketInteractor, handling Magazines that attach
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class MagazineInteractor : XRSocketInteractor
    {
        private const float EjectDetachDelay = .2f;
        
        [Space]
        [Header("Magazine Socket Settings")]
        public bool allowGrabMagazine;

        private readonly IXRSelectFilter _grabDetachFilter = new XRSelectFilterDelegate(MagazineGrabDetachFilter);
        
        private Firearm _firearm;
        
        /// <summary>
        /// Forces Magazine to Detach
        /// </summary>
        public void DetachMagazine()
        {
            if (interactablesSelected.Count == 0) return;
            
            //Detach interactable
            interactionManager.CancelInteractorSelection((IXRSelectInteractor)this);
        }

        /// <summary>
        /// Visually ejects magazine from weapon, still attached until it is detached
        /// </summary>
        public void EjectMagazine(bool scheduleDetach = false)
        {
            //Weapon can no longer use magazine
            //This also triggers detach animation
            _firearm.magazine = null;
            
            if (interactablesSelected.Count == 0) return;
            
            //Try remove filter that prevents grab
            SetGrabDetachFilter((XRBaseInteractable)interactablesSelected[0], false);
            
            if(scheduleDetach)
                Invoke(nameof(DetachMagazine), EjectDetachDelay);
        }

        /// <summary>
        /// Get the Magazine component of any currently attached interactables
        /// </summary>
        /// <returns>Attached magazine</returns>
        public Magazine GetAttachedMagazine()
        {
            if (interactablesSelected.Count == 0) return null;
            
            //Attached interactables should always have a Magazine component
            return interactablesSelected[0].transform.GetComponentInParent<Magazine>();
        }
        
        /// <summary>
        /// Called when a new magazine is attached
        /// </summary>
        /// <param name="args"></param>
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            
            _firearm.magazine = args.interactableObject.transform.GetComponent<Magazine>();//TODO can be replaced by GetAttachedMagazine()? is interactablesSelected[0] set by this time?
            
            //If we don't allow grabbing the magazine while we select it, apply filter that prevent this
            if(!allowGrabMagazine)
                SetGrabDetachFilter((XRBaseInteractable)args.interactableObject, true);
        }

        /// <summary>
        /// Called when magazine is detached
        /// </summary>
        /// <param name="args"></param>
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            
            _firearm.magazine = null;

            //Try remove potential grab detach Filter
            SetGrabDetachFilter((XRBaseInteractable)args.interactableObject, false);
        }
        
        /// <summary>
        /// Filter that makes sure new magazine is compatible with our weapon before attaching
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactable"></param>
        /// <returns></returns>
        private bool MagazineCompatibleFilter(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            Magazine magazine = interactable.transform.GetComponent<Magazine>();
            if (!magazine) return false;
            if (!magazine.IsCompatibleWithWeapon(_firearm)) return false;
            
            return true;
        }
        
        /// <summary>
        /// We apply this filter to interactables that we don't want to allow the player (or other interactors) to select
        /// while the MagazineInteractor is selecting it it
        /// </summary>
        /// <param name="interactor"></param>
        /// <param name="interactable"></param>
        /// <returns></returns>
        private static bool MagazineGrabDetachFilter(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
        {
            //Only allow interactions from magazine interactor
            return interactor is MagazineInteractor;
        }
        
        private void SetGrabDetachFilter(XRBaseInteractable interactable, bool filterEnabled)
        {
            if(filterEnabled)
                interactable.selectFilters.Add(_grabDetachFilter);
            else
                interactable.selectFilters.Remove(_grabDetachFilter);
        }

        protected override void Awake()
        {
            base.Awake();

            _firearm = GetComponentInParent<Firearm>();
            
            selectFilters.Add(new XRSelectFilterDelegate(MagazineCompatibleFilter));

            if (_firearm == null)
                Debug.LogError("Magazine trigger parent components were null");
        }
    }
}