using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    /// <summary>
    /// Prevents collision between the two rigidbodies that make up a socket interaction (interactor & interactable)
    /// </summary>
    [RequireComponent(typeof(XRSocketInteractor))]
    public class XRSocketInterCollisionPreventer : MonoBehaviour
    {
        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            Rigidbody ourRigidbody = GetComponentInParent<Rigidbody>();
            Rigidbody interactableRigidbody = args.interactableObject.transform.GetComponentInParent<Rigidbody>();
            
            if(ourRigidbody && interactableRigidbody)
                UpdateCollisionLayer(ourRigidbody, interactableRigidbody, true);
        }
        
        private void OnSelectExited(SelectExitEventArgs args)
        {
            Rigidbody ourRigidbody = GetComponentInParent<Rigidbody>();
            Rigidbody interactableRigidbody = args.interactableObject.transform.GetComponentInParent<Rigidbody>();
            
            if(ourRigidbody && interactableRigidbody)
                UpdateCollisionLayer(ourRigidbody, interactableRigidbody, false);
        }

        private static void UpdateCollisionLayer(Rigidbody rb1, Rigidbody rb2, bool preventInterCollision)
        {
            //Update physics layer of all children
            foreach (Collider rb1Collider in rb1.GetComponentsInChildren<Collider>())
                foreach (Collider rb2Collider in rb2.GetComponentsInChildren<Collider>())
                {
                    //No need to prevent collisions with triggers
                    if (rb1Collider.isTrigger || rb2Collider.isTrigger) continue;
                    
                    Physics.IgnoreCollision(rb1Collider, rb2Collider, preventInterCollision);
                }
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            GetComponent<XRSocketInteractor>().selectEntered.AddListener(OnSelectEntered);
            GetComponent<XRSocketInteractor>().selectExited.AddListener(OnSelectExited);
        }
    }
}
