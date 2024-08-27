using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VRFPSKit
{
    /// <summary>
    /// Represents a physical bullet that has been fired
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private const float ImpactEffectSearchRadius = .3f;
        
        public AudioSource hitSound;
        public GameObject defaultImpactEffect;
        public GameObject tracerTail;
        [Space]
        public BallisticProfile ballisticProfile;
        public BulletType bulletType;

        public BulletShooter shooter;
        
        //NOTE events will only be called on server
        public event Action<Bullet> HitEvent;
        public event Action<Bullet, Damageable> HitDamageableEvent;

        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            //Bullet apply randomSpreadAngle to rotation
            if(ballisticProfile.randomSpreadAngle != 0)
                transform.rotation *= Quaternion.Euler(
                    Random.Range(-ballisticProfile.randomSpreadAngle, ballisticProfile.randomSpreadAngle) , 
                    Random.Range(-ballisticProfile.randomSpreadAngle, ballisticProfile.randomSpreadAngle), 0);//TODO this doesnt work
            
            _rigidbody.AddForce(transform.forward * ballisticProfile.startVelocity, ForceMode.VelocityChange);
            _rigidbody.useGravity = false; //Use custom gravity solution
            
            tracerTail.SetActive(bulletType == BulletType.Tracer);
        }

        private void FixedUpdate()
        {
            //Custom gravity implementation
            _rigidbody.AddForce(Physics.gravity * ballisticProfile.gravityScale, ForceMode.Acceleration);
        }

        private void OnCollisionEnter(Collision other)
        {
            //Call Hit Event
            HitEvent?.Invoke(this);
            
            //Get damageable component in collider or in parents
            if (other.gameObject.GetComponentInParent<Damageable>() is Damageable damageable)
            {
                //Apply damage to damageable
                damageable.TakeDamage(ballisticProfile.baseDamage);
                
                //Call Hit Damageable Event
                HitDamageableEvent?.Invoke(this, damageable);
            }
            //Stop moving
            _rigidbody.isKinematic = true;

            //Play hit effects on clients
            RPC_PlayImpactEffect(other.GetContact(0).point, other.GetContact(0).normal);

            //Schedule destruction, let sound play out first
            Invoke(nameof(DestroyBullet), 1);
        }

        private void RPC_PlayImpactEffect(Vector3 impactPoint, Vector3 impactNormal)
        {
            hitSound.Play();
            
            //Hide bullet renderer on all clients
            foreach(MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
                Destroy(renderer);

            //Locally spawn the particle effect
            Quaternion normalQuaternion = Quaternion.LookRotation(impactNormal);
            GameObject impactEffectPrefab = GetImpactEffect(impactPoint);
            
            GameObject impactEffectObj = Instantiate(impactEffectPrefab, impactPoint, normalQuaternion);
        }

        /// <summary>
        /// Method that tries to fetch a BulletImpactEffect component on nearby colliders.
        /// Doing it this way allows for clients to decide which effect to use on their own,
        /// instead of having to network impact effects (complicated since we can't easily send
        /// Prefab references over the network).
        /// </summary>
        /// <param name="impactPoint"></param>
        /// <returns></returns>
        private GameObject GetImpactEffect(Vector3 impactPoint)
        {
            //Try to find active BulletImpactEffect components near where the bullet hit
            foreach (Collider nearbyCollider in Physics.OverlapSphere(impactPoint, ImpactEffectSearchRadius))
                if(nearbyCollider.GetComponentInParent<BulletImpactEffect>() is BulletImpactEffect bulletImpactEffect && 
                   bulletImpactEffect.impactEffect != null)
                    //If one was found, use it's particle effect instead
                    return bulletImpactEffect.impactEffect;
                
            //If no BulletImpactEffect was found, use the default impact effect
            return defaultImpactEffect;
        }

        private void DestroyBullet()
        {
            Destroy(gameObject);
        }
    }
}