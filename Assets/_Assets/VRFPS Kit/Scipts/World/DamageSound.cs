using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(Damageable))]
    public class DamageSound : MonoBehaviour
    {
        public AudioSource audioSource;
        public bool onlyPlayOnDeath;

        private float _healthLastFrame;
        
        private Damageable _damageable;
        
        // Update is called once per frame
        void Update()
        {
            //Play sound when we have lost health (health is less than it was last frame)   
            if (_damageable.health >= _healthLastFrame) return;
            //If onlyPlayOnDeath is enabled, check if damageable is dead
            if (onlyPlayOnDeath && _damageable.health > 0) return;
            
            audioSource.Play();
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