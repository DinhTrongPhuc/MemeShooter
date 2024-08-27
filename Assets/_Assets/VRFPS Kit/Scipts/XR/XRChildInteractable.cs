using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

namespace VRFPSKit
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class XRChildInteractable : MonoBehaviour
    {
        private XRBaseInteractable _parentInteractable;
        private XRBaseInteractable[] _interactablesOnObject;

        // Update is called once per frame
        void Update()
        {
            bool isHeld = false;

            //Check if object is being held by a hand
            if (_parentInteractable.isSelected)
                //Check if holder is a hand
                if (_parentInteractable.interactorsSelecting[0] is XRDirectInteractor)
                    isHeld = true;

            //Update active state
            foreach (XRBaseInteractable childInteractable in _interactablesOnObject)
            {
                childInteractable.enabled = isHeld;
            }
        }

        private void Awake()
        {
            //Get interactable in parent, transform.parent ensures we aren't fetching the interactable on our local object
            _parentInteractable = transform.parent.GetComponentInParent<XRBaseInteractable>();
            _interactablesOnObject = GetComponents<XRBaseInteractable>();

            foreach (var interactable in _interactablesOnObject)
            {
                interactable.interactionStrengthFilters.Add(new XRInteractionStrengthFilterDelegate(PrioritizedChildStrengthFilter));
            }
        }
        
        private float PrioritizedChildStrengthFilter(IXRInteractor interactor, IXRInteractable interactable, float interactionStrength)
        {
            return float.MaxValue;
        }
    }
}