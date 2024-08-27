using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    /// <summary>
    /// Replicates a feature most often found on german firearms like the HK Mp5 or the MP40,
    /// where the action can be manually locked back by moving the charging handle up in to a notch
    /// which physically holds the bolt back.
    /// </summary>
    [RequireComponent(typeof(FirearmCyclingActionInteractable))]
    public class PhysicalActionManualLockback : MonoBehaviour
    {
        public float minimumActionPosition01 = .8f;
        public Vector3 lockbackMovement;
        
        private Vector3 _handRelativePositionLastFrame;
        private float _deltaMovementMagnitude;

        private XRSimpleInteractable _fireSelectInteractable;
        private FirearmCyclingActionInteractable _actionInteractable;

        // Update is called once per frame
        void Update()
        {
            //Only run when holding action
            if (!_fireSelectInteractable.isSelected) return;
            
            TrackHandMovement();
            TryLockAction();
        }

        /// <summary>
        /// Changes to the next fire mode once hand has moved far enough along specified direction
        /// </summary>
        private void TryLockAction()
        {
            //Wait until minimum action position has been reached
            if (_actionInteractable.cyclingAction.actionPosition01 < minimumActionPosition01) return;
            //Wait until hand has moved enough to trigger lockback
            if (_deltaMovementMagnitude < lockbackMovement.magnitude) return;

            //Reset delta movement
            _deltaMovementMagnitude = 0;
            
            _actionInteractable.cyclingAction.isLockedBack = true;
            _actionInteractable.ForceDetachSelectors();
        }

        private void TrackHandMovement()
        {
            //return if _handPositionLastFrame isn't initialized
            if (_handRelativePositionLastFrame == Vector3.zero) return;
            
            //Calculate hand movement change since last frame
            Vector3 deltaHandMovement = GetHandRelativePosition() - _handRelativePositionLastFrame;
            //Translate hand movement to fire mode movement direction
            Vector3 trackedDirectionDeltaMovement =
                Vector3.Scale(deltaHandMovement, lockbackMovement.normalized);
            
            _deltaMovementMagnitude += trackedDirectionDeltaMovement.x + trackedDirectionDeltaMovement.y +
                                       trackedDirectionDeltaMovement.z;
        }

        private Vector3 GetHandRelativePosition()
        {
            if(!_fireSelectInteractable.isSelected) return Vector3.zero;
            
            return transform.InverseTransformPoint(_fireSelectInteractable.interactorsSelecting[0].transform.position);
        }
        
        private void LateUpdate()
        {
            _handRelativePositionLastFrame = GetHandRelativePosition();
        }

        // Start is called before the first frame update
        void Awake()
        {
            //Fetch components necessary
            _fireSelectInteractable = GetComponent<XRSimpleInteractable>();
            _actionInteractable = GetComponentInParent<FirearmCyclingActionInteractable>();
        }
    }
}