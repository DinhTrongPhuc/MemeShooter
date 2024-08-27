using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    [RequireComponent(typeof(AudioSource))]
    public class FootstepProducer : MonoBehaviour
    {
        public float distancePerSound;

        private Vector3 _lastSoundPosition;
        private AudioSource _source;
        private CharacterController _movement;
        

        /// <summary>
        /// Plays footstep sound every x meters, as long as player is grounded
        /// </summary>
        void Update()
        {
            if (!_movement.isGrounded)
                return;
            if (Vector3.Distance(transform.position, _lastSoundPosition) < distancePerSound)
                return;

            _source.Play();
            _lastSoundPosition = transform.position;
        }

        void Awake()
        {
            _movement = GetComponentInParent<CharacterController>();
            _source = GetComponent<AudioSource>();
        }
    }
}