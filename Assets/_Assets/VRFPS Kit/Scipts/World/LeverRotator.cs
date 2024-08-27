using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRMultiplayerFPSKit
{
    public class LeverRotator : MonoBehaviour
    {
        public Vector3 focalFullRotation;
        
        private Lever _lever;
        private Quaternion _startRotation;

        // Update is called once per frame
        void Update()
        {
            //Calculate the amount we should rotate from the "_startRotation" based on how large the "value01" is
            Quaternion leverValueRotation = Quaternion.Euler(focalFullRotation * _lever.value01);
            //Calculate the final local rotation by adding the leverValueRotation to the _startRotation
            Quaternion finalLeverRotation = _startRotation * leverValueRotation;

            transform.localRotation = finalLeverRotation;
        }
        
        void Awake()
        {
            _lever = GetComponentInParent<Lever>();

            _startRotation = transform.localRotation;
        }
    }
}
