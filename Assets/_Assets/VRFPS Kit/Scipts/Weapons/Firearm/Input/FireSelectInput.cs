using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VRFPSKit.Input;

namespace VRFPSKit
{
    /// <summary>
    /// Switches Firearm firemode when input is pressed
    /// </summary>
    [RequireComponent(typeof(Firearm), typeof(XRGrabInteractable))]
    public class FireSelectInput : MonoBehaviour
    {
        public XRHeldInputAction fireModeInput;
        
        private bool _actionLockedLastFrame;

        private XRGrabInteractable _grabbable;
        private Firearm _firearm;
        
        // Update is called once per frame
        void Update()
        {
            TrySwitchFireMode();
        }

        private void TrySwitchFireMode()
        {
            if (!_grabbable.isSelected) return;
            //unlocking action uses same button, prevent both inputs from occuring at the same time
            if(_actionLockedLastFrame) return;
            
            InputAction input = fireModeInput.GetActionForPrimaryHand(_grabbable);

            //If no input action is active, return
            if (input == null) return;
            if (!input.WasPressedThisFrame()) return;
            
            _firearm.currentFireMode = NextFireMode();
        }
        
        private FireMode NextFireMode()
        {
            int currentFireModeIndex = 0;

            if (_firearm.availableFireModes.Contains(_firearm.currentFireMode))
                currentFireModeIndex = Array.IndexOf(_firearm.availableFireModes, _firearm.currentFireMode);

            int nextFireModeIndex = currentFireModeIndex + 1;
            //Wrap around back to 0 if index is outside list
            nextFireModeIndex %= _firearm.availableFireModes.Count();
            
            return _firearm.availableFireModes[nextFireModeIndex];
        }

        private void LateUpdate()
        {
            _actionLockedLastFrame =
                (GetComponent<FirearmCyclingAction>() && GetComponent<FirearmCyclingAction>().isLockedBack);
        }

        // Start is called before the first frame update
        void Awake()
        {
            _grabbable = GetComponent<XRGrabInteractable>();
            _firearm = GetComponent<Firearm>();
        }
    }
}