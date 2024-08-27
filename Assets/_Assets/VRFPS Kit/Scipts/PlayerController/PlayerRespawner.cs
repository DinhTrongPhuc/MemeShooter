using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Teleports player once Damageable health reaches 0
    /// </summary>
    [RequireComponent(typeof(Damageable))]
    public class PlayerRespawner : MonoBehaviour
    {
        private Damageable _damageable;
        private float _healthLastFrame;
        
        private void Awake()
        {
            _damageable = GetComponent<Damageable>();
        }

        void Update()
        {
            if(_damageable.health != _healthLastFrame && _damageable.health <= 0)
                Respawn();

            _healthLastFrame = _damageable.health;
        }
        
        private void Respawn()
        {
            Vector3 spawnPosition = Vector3.zero;
            
            transform.position = spawnPosition;
            GetComponent<Damageable>().ResetHealth();
        }
    }
}