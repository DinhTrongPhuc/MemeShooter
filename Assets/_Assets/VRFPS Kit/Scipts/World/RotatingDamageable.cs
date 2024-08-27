using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    [RequireComponent(typeof(Damageable))]
    public class RotatingDamageable : MonoBehaviour
    {
        public Transform rotator;
        public Vector3 destroyedRotation;

        private Damageable _damageable;
        private Quaternion _defaultRotation;

        // Start is called before the first frame update
        void Start()
        {
            _damageable = GetComponent<Damageable>();
            _defaultRotation = rotator.localRotation;
        }

        // Update is called once per frame
        void Update()
        {
            //Set rotation on clients based on whether damageable is destroyed
            rotator.localRotation =
                (_damageable.health <= 0) ? Quaternion.Euler(destroyedRotation) : _defaultRotation;
        }
    }
}