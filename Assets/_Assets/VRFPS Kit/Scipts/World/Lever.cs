using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

#if HAPTIC_PATTERNS
using HapticPatterns;
#endif

namespace VRMultiplayerFPSKit
{
    public class Lever : MonoBehaviour
    {
        #if HAPTIC_PATTERNS
        public HapticPattern hapticPattern;
        #else
        [Header("VR Haptic Patterns Isn't Installed")]
        #endif
        
        public Vector3 localLeverDirection;
        public float returnToBaseSpeed;
        [Space]
        public float value01;
        public bool activated;
        [Space]
        [Header("Events are only called on the server")]
        public UnityEvent leverActivateEvent;
        public UnityEvent leverDeactivateEvent;

        private XRSimpleInteractable _interactable;
        private Vector3 _handRelativePositionLastFrame;

        private void HeldUpdate()
        {
            float newLeverValue = Mathf.Clamp01(value01 + HandMovementDeltaMagnitude());
            value01 = newLeverValue;
            
            #if HAPTIC_PATTERNS
                hapticPattern.PlayGradually(_interactable, value01);
            #endif
        }

        private float HandMovementDeltaMagnitude()
        {
            //return if _handPositionLastFrame isn't initialized
            if (_handRelativePositionLastFrame == Vector3.zero) return 0;
            
            //Calculate hand movement change since last frame
            Vector3 deltaHandMovement = GetHandRelativePosition() - _handRelativePositionLastFrame;
            //Translate hand movement to bolt position change
            Vector3 trackedDirectionDeltaMovement = Vector3.Scale(
                deltaHandMovement,
                localLeverDirection);
            
            float movementMagnitude = trackedDirectionDeltaMovement.x + trackedDirectionDeltaMovement.y +
                                      trackedDirectionDeltaMovement.z;
            
            //Apply bolt position change
            return movementMagnitude;
        }

        private Vector3 GetHandRelativePosition()
        {
            IXRSelectInteractor hand = GetHand();
            if(hand == null) return Vector3.zero;
            
            return transform.InverseTransformPoint(hand.transform.position);
        }

        private IXRSelectInteractor GetHand()
        {
            if (!_interactable.isSelected) return null;
            
            return _interactable.interactorsSelecting[0];
        }

        // Update is called once per frame
        private void Update()
        {
            if (_interactable.isSelected) HeldUpdate();
            
            bool activatedThisFrame = value01 > .5f;
            
            //Did Activated state just change?
            if (activatedThisFrame != activated)
            {
                activated = activatedThisFrame;
                
                //Call event based on new activated state
                if(activated)
                    leverActivateEvent.Invoke();
                else
                    leverDeactivateEvent.Invoke();
            }

            float returnToBaseMovement = returnToBaseSpeed * Time.deltaTime * (activated ? 1 : -1);
            value01 = Mathf.Clamp01(value01 + returnToBaseMovement);
        }

        private void LateUpdate()
        {
            _handRelativePositionLastFrame = GetHandRelativePosition();
        }

        private void Awake()
        {
            _interactable = GetComponentInChildren<XRSimpleInteractable>();
        }
    }
}