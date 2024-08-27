using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(Damageable))]
    public class FlipTarget : MonoBehaviour
    {
        public Transform rotator;
        public Vector3 localFlippedRotation;
        public float flipSpeed;

        private bool _flipped;
        
        private Damageable _damageable;
        private Quaternion _startRotation;

        // Update is called once per frame
        void Update()
        {
            //Set rotation on clients based on whether damageable is destroyed
            Quaternion targetRotation = _flipped ? Quaternion.Euler(localFlippedRotation) : _startRotation;
            
            //Rotate towards the targetRotation, taking flipSpeed in to account
            rotator.localRotation = Quaternion.RotateTowards(rotator.localRotation, targetRotation, flipSpeed * Time.deltaTime);
        }
        
        private void DamageEvent(float damage)
        {
            //Flip when damaged
            _flipped = !_flipped;
        }

        private void ResetHealthEvent()
        {
            //Reset flip state when health is reset
            _flipped = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            _damageable = GetComponent<Damageable>();
            
            _startRotation = rotator.localRotation;
            SubscribeToDamageableEvents();
        }

        private void SubscribeToDamageableEvents()
        {
            _damageable.DamageEvent += DamageEvent;
            _damageable.ResetHealthEvent += ResetHealthEvent;
        }
    }
}