using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    /// <summary>
    /// Simply tracks health, allowing for further behaviour extention by composition
    /// </summary>
    public class Damageable : MonoBehaviour
    {
        
        public float health;
        [HideInInspector] public float startHealth;
        
        //NOTE events will only be called on server
        public static event Action<Damageable> GlobalDeathEvent;
        public static event Action<Damageable, float> GlobalDamageEvent;
        public event Action<float> DamageEvent;
        public event Action DeathEvent;
        public event Action ResetHealthEvent;

        private void Start()
        {
            startHealth = health;
        }
        
        public void TakeDamage(float damage)
        {

            health -= damage;
            DamageEvent?.Invoke(damage);
            GlobalDamageEvent?.Invoke(this, damage);

            if (health <= 0)
            {
                DeathEvent?.Invoke();
                GlobalDeathEvent?.Invoke(this);
            }
        }
        
        public void ResetHealth()
        {
            health = startHealth;
            ResetHealthEvent?.Invoke();
        }

        
    }
}