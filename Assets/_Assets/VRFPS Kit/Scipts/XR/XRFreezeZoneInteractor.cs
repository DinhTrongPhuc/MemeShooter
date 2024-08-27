using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    public class XRFreezeZoneInteractor : MonoBehaviour
    {
        private readonly List<XRGrabInteractable> _trackedInteractables = new();
        private readonly Dictionary<Rigidbody, RigidbodyConstraints> _previousConstraints = new();

        private void Update()
        {
            foreach (XRGrabInteractable interactable in _trackedInteractables)
            {
                //Null check, in case interactable has been destroyed
                if (interactable == null)
                    continue;

                Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
                
                if(!rigidbody)
                    continue;

                //TODO figure out how to use kinematic mode instead, without causing troubles with other interactors using the wrong mode
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            XRGrabInteractable interactable = other.GetComponentInParent<XRGrabInteractable>();

            if (!interactable)
                return;
            Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
            if (!rigidbody)
                return;
            if (_trackedInteractables.Contains(interactable))
                return;

            _trackedInteractables.Add(interactable);

            
            _previousConstraints.Add(rigidbody, rigidbody.constraints);
        }

        private void OnTriggerExit(Collider other)
        {
            XRGrabInteractable interactable = other.GetComponentInParent<XRGrabInteractable>();

            if (!interactable)
                return;
            if (!_trackedInteractables.Contains(interactable))
                return;

            _trackedInteractables.Remove(interactable);

            Rigidbody rigidbody = interactable.GetComponent<Rigidbody>();
            if (rigidbody && _previousConstraints.ContainsKey(rigidbody))
                rigidbody.constraints = _previousConstraints[rigidbody];
        }
    }
}