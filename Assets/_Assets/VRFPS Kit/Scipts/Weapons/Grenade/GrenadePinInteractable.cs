using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

#if HAPTIC_PATTERNS
using HapticPatterns;
#endif

namespace VRFPSKit
{
    /// <summary>
    /// Creates a proxy interactable for the Grenade Pin, allowing you to pull it in order to detach it.
    /// </summary>
    [RequireComponent(typeof(XRSimpleInteractable))]
    public class GrenadePinInteractable : MonoBehaviour
    {
        #if HAPTIC_PATTERNS
        public HapticPattern pinPullPattern;
        #else
        [Header("VR Haptic Patterns Isn't Installed")]
        #endif
        
        public Vector3 pinPullDistanceLocalSpace;

        private float _pinPullProgress01;
        private Vector3 _handPositionLastFrame;
        private Vector3 _startingPosition;

        private Grenade _grenade;
        private XRSimpleInteractable _pinInteractable;

        // Update is called once per frame
        void Update()
        {
            PinTrackHandMovement();
            PositionPinVisually();

            //If pin is pulled completely, detach
            if (_pinPullProgress01 > .99f)
                DetachPin();
            
            #if HAPTIC_PATTERNS
            if (_pinInteractable.isSelected)
                pinPullPattern.PlayGradually(_pinInteractable, _pinPullProgress01);
            #endif
        }

        /// <summary>
        /// Tracks hand movement to determine _pinPullProgress01
        /// </summary>
        private void PinTrackHandMovement()
        {
            //return 0 if not held
            if (!_pinInteractable.isSelected) return;
            //return 0 if wasn't held last frame
            if (_handPositionLastFrame == Vector3.zero) return;

            Vector3 currentHandPosition = _pinInteractable.interactorsSelecting[0].transform.position;

            //Calculate hand movement change since last frame
            Vector3 deltaHandMovement = currentHandPosition - _handPositionLastFrame;
            Vector3 deltaHandMovementInLocalSpace = transform.InverseTransformVector(deltaHandMovement);

            //Translate hand movement to pin position change
            Vector3 pinPullDirectionLocalSpace = pinPullDistanceLocalSpace.normalized;
            Vector3 trackedDirectionDeltaMovement =
                Vector3.Scale(-deltaHandMovementInLocalSpace, pinPullDirectionLocalSpace);
            float pullMagnitude = trackedDirectionDeltaMovement.x + trackedDirectionDeltaMovement.y +
                                  trackedDirectionDeltaMovement.z;

            //Scale magnitude to the total pull distance
            pullMagnitude /= pinPullDistanceLocalSpace.magnitude;
            pullMagnitude *= -1;

            _pinPullProgress01 = Mathf.Clamp01(_pinPullProgress01 + pullMagnitude);
        }

        /// <summary>
        /// Visually moves the pin object according to _pinPullProgress01 value
        /// </summary>
        private void PositionPinVisually()
        {
            transform.localPosition = _startingPosition + (_pinPullProgress01 * pinPullDistanceLocalSpace);
        }

        private void DetachPin()
        {
            _grenade.DetachSafetyPin();
            
            //Destroy components related to pulling it, so this behaviour no longer is possible
            Destroy(GetComponent<XRChildInteractable>());
            Destroy(this);
            Destroy(_pinInteractable);
        }

        // Start is called before the first frame update
        void Awake()
        {
            //Fetch components necessary
            _grenade = GetComponentInParent<Grenade>();
            _pinInteractable = GetComponent<XRSimpleInteractable>();
            _startingPosition = transform.localPosition;

            if (_grenade == null)
                Debug.LogError("Grenade pin has no grenade parent!");
        }

        private void LateUpdate()
        {
            if (_pinInteractable.isSelected)
                _handPositionLastFrame = _pinInteractable.interactorsSelecting[0].transform.position;
            else
                _handPositionLastFrame = Vector3.zero;
        }
    }
}