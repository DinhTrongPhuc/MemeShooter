using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VRFPSKit.Input;

namespace VRFPSKit
{
    /// <summary>
    /// Unlocks Locked action when hand enters trigger, perfect for implementing
    /// AR-15 Bolt release button or the "H&K MP5 Slap"
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class PhysicalUnlockActionTrigger : MonoBehaviour
    {
        private XRGrabInteractable _interactable;
        private FirearmCyclingAction _cyclingAction;

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            //If not held, return
            if (!_interactable.isSelected) return;
            //If weapon action isn't locked back, return
            if (!_cyclingAction.isLockedBack) return;
            
            if (other.gameObject.layer != LayerMask.NameToLayer("HandPresence")) return;

            _cyclingAction.TryUnlockAction();
        }

        // Start is called before the first frame update
        void Awake()
        {
            _interactable = GetComponentInParent<XRGrabInteractable>();
            _cyclingAction = GetComponentInParent<FirearmCyclingAction>();

            if(!_interactable)
                Debug.LogError("No XRGrabInteractable was found in parent, component won't work");
            if(!_cyclingAction)
                Debug.LogError("No CyclingWeaponAction was found in parent, component won't work");

            if (Physics.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("HandPresence")))
                Debug.LogError($"{gameObject.name} is on a layer that won't collide with player HandPresence, UnlockActionPhysicalCollider won't work, suggest using layer 'Default'");
        }
    }
}