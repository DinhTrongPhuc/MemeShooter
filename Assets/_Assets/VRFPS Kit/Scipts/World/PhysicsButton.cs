using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    public class PhysicsButton : MonoBehaviour
    {
        public float threshold01 = .8f;
        
        [Space] 
        public bool pressed;
        public UnityEvent wasPressedEvent;

        private Vector3 _startPos;
        private ConfigurableJoint _joint;
        private float _lastPressed;

        // Update is called once per frame
        void Update()
        {
            //Determine if button is pressed this frame
            pressed = GetValue() > threshold01;

            //If button was just pressed, call event
            if (pressed && Time.time - _lastPressed > 1)
            {
                wasPressedEvent.Invoke();

                _lastPressed = Time.time;
            }
        }

        private float GetValue()
        {
            float value = Vector3.Distance(_startPos, _joint.transform.position) / _joint.linearLimit.limit;

            return Mathf.Clamp(value, 0, 1f);
        }

        private void Awake()
        {
            _joint = GetComponentInChildren<ConfigurableJoint>();
            _startPos = _joint.transform.position;
        }
    }
}