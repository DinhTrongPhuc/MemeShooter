using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VRFPSKit.Input;
using VRFPSKit;
    
#if HAPTIC_PATTERNS
using HapticPatterns;
#endif

namespace VRFPSKit
{
    /// <summary>
    /// Handles all Firearm Trigger Behaviour, Firemodes are also implemented here
    /// </summary>
    [RequireComponent(typeof(Firearm), typeof(XRGrabInteractable))]
    public class FirearmTrigger : MonoBehaviour
    {
        #if HAPTIC_PATTERNS
        public HapticPattern triggerPressPattern;
        public HapticPattern triggerResetPattern;
        #else
        [Header("VR Haptic Patterns Isn't Installed")]
        #endif
        
        [Space]
        public XRHeldInputAction triggerPressed01Input;
        
        [Space]
        [Range(0, 1)]public float triggerPressThreshold = .5f;
        [Range(0, 1)]public float triggerResetThreshold = .5f;

        private float _triggerProgress01;
        private bool _triggerWaitingForReset;
        
        private XRGrabInteractable _grabbable;
        private Firearm _firearm;
        
        // Update is called once per frame
        void Update()
        {
            if (!_grabbable.isSelected) return;
            
            InputAction triggerInput = triggerPressed01Input.GetActionForPrimaryHand(_grabbable);
            if (triggerInput == null) return;
            
            //Read trigger input
            _triggerProgress01 = triggerInput.ReadValue<float>();

            TriggerHaptics();
            TryFire();
            
            //Reset trigger if it is released enough
            if (_triggerProgress01 < triggerResetThreshold)
                _triggerWaitingForReset = false;
        }

        private void TryFire()
        {
            //Can't fire if fire mode is Safe
            if (_firearm.currentFireMode == FireMode.Safe) return;
            //Cant fire if trigger hasn't reset yet
            if (_triggerWaitingForReset) return;
            //Trigger press strength must exceed threshold
            if (_triggerProgress01 < triggerPressThreshold) return;
            
            _firearm.TryShoot();
            
            //If fire mode is single fire, wait for trigger reset before firing again
            if (_firearm.currentFireMode == FireMode.Single_Fire)
                _triggerWaitingForReset = true;
        }

        private void TriggerHaptics()
        {
            #if HAPTIC_PATTERNS
            if(_triggerWaitingForReset)
                triggerResetPattern.PlayGradually(_grabbable, _triggerProgress01);
            else
                triggerPressPattern.PlayGradually(_grabbable, _triggerProgress01);
            #endif
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            _grabbable = GetComponent<XRGrabInteractable>();
            _firearm = GetComponent<Firearm>();
        }
    }
}