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
    /// Releases Locked Action when input is pressed
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable), typeof(FirearmCyclingAction))]
    public class UnlockActionInput : MonoBehaviour
    {
        public XRHeldInputAction actionReleaseInput;

        private XRGrabInteractable _interactable;
        private FirearmCyclingAction _cyclingAction;

        // Update is called once per frame
        void Update()
        {
            //If not held, return
            if (!_interactable.isSelected) return;
            //If weapon action isn't locked back, return
            if (!_cyclingAction.isLockedBack) return;

            //Get input action for the hand currently holding
            InputAction unlockAction = actionReleaseInput.GetActionForPrimaryHand(_interactable);

            //If no input action is active, return
            if (unlockAction == null) return;
            if (!unlockAction.IsPressed()) return;
            
            _cyclingAction.TryUnlockAction();
        }

        // Start is called before the first frame update
        void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _cyclingAction = GetComponent<FirearmCyclingAction>();
        }
    }
}