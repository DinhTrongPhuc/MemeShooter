using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(Rigidbody), typeof(Damageable))]
    public class Skeet : MonoBehaviour
    {
        private const float MaxLifetime = 10;
        
        public float startVelocity;
        public float liftMultiplier = 1;

        private Rigidbody _rigidbody;
        private Damageable _damageable;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _damageable = GetComponent<Damageable>();
            
            //Prevent build up of undestroyed objects
            Destroy(gameObject, MaxLifetime);
            
            //Add start velocity
            _rigidbody.AddForce(transform.forward * startVelocity, ForceMode.VelocityChange);
        }

        private void FixedUpdate()
        {
            RemoveTrailsWhenDead();
            
            //Add lift force in upwards direction
            _rigidbody.AddForce(Physics.gravity.magnitude * liftMultiplier * transform.up, ForceMode.Acceleration);

            //Rotate so that the y-axis is facing in the direction of travel, will achieve a glide movement
            transform.rotation *= Quaternion.FromToRotation(transform.forward, _rigidbody.velocity.normalized);
        }

        /// <summary>
        /// Kill the object when it collides with anything
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            //We do not care about collisions after death
            if (_damageable.health <= 0) return;
            
            _damageable.TakeDamage(100);
            _rigidbody.isKinematic = true;
        }

        /// <summary>
        /// A quick fix method for the fact that the skeet trails would
        /// keep rendering after death
        /// </summary>
        private void RemoveTrailsWhenDead()
        {
            if (_damageable.health > 0) return;

            foreach (TrailRenderer trail in gameObject.GetComponentsInChildren<TrailRenderer>())
                Destroy(trail);
        }
    }
}
