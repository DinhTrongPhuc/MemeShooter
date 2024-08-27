using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    /// <summary>
    /// Main class that handles behaviour that is in common for all grenades
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
    public class Grenade : MonoBehaviour
    {
        public float fuseDuration;
        public GameObject explosionPrefab;
        [Space] 
        public Rigidbody safetyPin;
        public Rigidbody lever;
        [Space] 
        public AudioSource primerSound;
        public AudioSource pinPullSound;

        private double _primedTime = -1;
        
        // Update is called once per frame
        void Update()
        {
            TryDetachLever();
            
            //If is primed (_primedTime isn't = -1 anymore) & fuse duration has expired, explode
            if (_primedTime > 0 && Time.time - _primedTime > fuseDuration)
                Explode();
        }

        public void DetachSafetyPin()
        {
            DetachSafetyPinClientEffects();
        }
        
        private void TryDetachLever()
        {
            //Wait until pin is detached
            if (safetyPin) return;
            //Dont try detaching lever if it already has been done
            if (!lever) return;
            //Don't try detaching if already primed
            if (_primedTime > 0) return;
            //Wait until rigidbody isn't kinematic, meaning no one is holding it
            if (GetComponent<Rigidbody>().isKinematic) return;

            DetachLeverClientEffects();
            
            //Detach if all conditions are met
            Prime();
        }

        private void Prime()
        {
            //Schedule explosion
            _primedTime = Time.time;
        }
        
        private void Explode()
        {
            //Spawn explosion at our position
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            //Destroy grenade
            Destroy(gameObject);
        }
        
        #region Client Effects
        private void DetachSafetyPinClientEffects()
        {
            //Play pin pull sound
            pinPullSound.Play();

            //Safety pin is no longer part of grenade
            safetyPin.transform.parent = null;

            //Enable physics on the pin, and reset velocity
            safetyPin.isKinematic = false;
            safetyPin.velocity = Vector3.zero;

            //Schedule safety pin object destruction
            Destroy(safetyPin.gameObject, 10);

            safetyPin = null;
        }
        
        private void DetachLeverClientEffects()
        {
            //Play pin pull sound
            primerSound.Play();

            //Lever is no longer part of grenade
            lever.transform.parent = null;

            //Enable physics on the lever, and use same velocity as grenade
            lever.isKinematic = false;
            lever.velocity = GetComponent<Rigidbody>().velocity * .3f;
            //TODO random angular vel

            //Schedule lever object destruction
            Destroy(lever.gameObject, 5);

            lever = null;
        }
        #endregion
    }
}