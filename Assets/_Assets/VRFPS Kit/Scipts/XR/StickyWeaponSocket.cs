using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRMultiplayerFPSKit
{
    /// <summary>
    /// Apply this script to a Socket object on the player in order to make it "sticky"
    /// Can be used to make weapons return to socket whenever player drops it
    /// </summary>
    [RequireComponent(typeof(XRSocketInteractor))]
    public class StickyWeaponSocket : MonoBehaviour
    {
        private XRSocketInteractor _socket;
        
        // Start is called before the first frame update
        void Awake()
        {
            _socket = GetComponent<XRSocketInteractor>();
            
            SubscribeToHandEvents();
        }

        private void SubscribeToHandEvents()
        {
            GameObject playerObj = GetComponentInParent<CharacterController>().gameObject;

            foreach (XRDirectInteractor playerHand in playerObj.GetComponentsInChildren<XRDirectInteractor>())
                playerHand.selectExited.AddListener(OnHandReleaseObject);
        }

        private void OnHandReleaseObject(SelectExitEventArgs selectExitEvent)
        {
            IXRSelectInteractable releasedInteractable = selectExitEvent.interactableObject;
            
            //If we can't select the interactable that the player released, ignore it
            if (!releasedInteractable.IsSelectableBy(_socket)) return;
            
            //Deselect everything in preparation
            _socket.interactionManager.CancelInteractorSelection((IXRSelectInteractor)_socket);
            //Select released object
            _socket.interactionManager.SelectEnter(_socket, releasedInteractable);
        }
    }
}
