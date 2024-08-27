using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    /// <summary>
    /// Updates the animator controller variables
    /// </summary>
    [RequireComponent(typeof(Firearm),
        typeof(Animator))]
    public class FirearmAnimator : MonoBehaviour
    {
        public AudioSource shootSound;
        public ParticleSystem shootParticle;
        [Space]

        private Firearm _firearm;
        private Animator _animator;
        private FirearmCyclingAction _cyclingAction;

        // Update is called once per frame
        void Update()
        {
            if (_cyclingAction)
            {
                _animator.SetFloat("Action Position",
                    //Clamp between 0 & .99 since animation will overflow back to 0 if value reaches 1
                    Mathf.Clamp(_cyclingAction.actionPosition01, 0, .99f));
                
                //Only run this on weapons that can be locked back
                _animator.SetBool("Action Locked Back", _cyclingAction.isLockedBack);
            }

            _animator.SetBool("Magazine Attached", _firearm.magazine != null);
            _animator.SetInteger("Fire Mode Index", 
                Array.IndexOf(_firearm.availableFireModes, _firearm.currentFireMode));
        }
        
        private async void Shoot(Cartridge cartridge)
        {
            //Wait until next frame before playing animations:
            //Playing recoil animation before spawning bullet might mean it gets shot at the wrong angle
            await Task.Delay((int)(Time.deltaTime*1000));
            
            _animator.SetTrigger("Shoot");
            ShootEffects();
        }
        
        private void ShootEffects()
        {
            if (shootSound)
                shootSound.Play();
            
            if (shootParticle)
                shootParticle.Play();
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            _firearm = GetComponent<Firearm>();
            _animator = GetComponent<Animator>();
            _cyclingAction = GetComponent<FirearmCyclingAction>();

            _firearm.ShootEvent += Shoot;
        }
    }
}