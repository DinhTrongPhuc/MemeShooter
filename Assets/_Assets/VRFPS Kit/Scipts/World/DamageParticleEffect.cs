using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(Damageable))]
    public class DamageParticleEffect : MonoBehaviour
    {
        public ParticleSystem particle;
        public bool onlyPlayOnDeath;
        
        private Damageable _damageable;
        
        private float _healthLastFrame;

        // Update is called once per frame
        void Update()
        {
            //Play effect when we have lost health (health is less than it was last frame)   
            if (_damageable.health >= _healthLastFrame) return;
            //If onlyPlayOnDeath is enabled, check if damageable is dead
            if (onlyPlayOnDeath && _damageable.health > 0) return;
            
            //Set visibility on clients based on whether damageable is destroyed
            particle.Play();
        }

        private void LateUpdate()
        {
            _healthLastFrame = _damageable.health;
        }

        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
        }
    }
}