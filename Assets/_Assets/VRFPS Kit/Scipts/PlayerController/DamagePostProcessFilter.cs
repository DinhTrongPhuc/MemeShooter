using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace VRFPSKit
{
    [RequireComponent(typeof(PostProcessVolume))]
    public class DamagePostProcessFilter : MonoBehaviour
    {
        public AnimationCurve profileHealthWeightCurve = AnimationCurve.Linear(0, 1, 1, 0);
        
        private PostProcessVolume _volume;
        private Damageable _damageable;

        // Update is called once per frame
        void Update()
        {
            //Full HP should result in 0 weight, and vice versa
            float health01 = _damageable.health / _damageable.startHealth;
            float weight = profileHealthWeightCurve.Evaluate(health01);
            _volume.weight = weight;
        }

        private void Awake()
        {
            _volume = GetComponent<PostProcessVolume>();
            _damageable = GetComponentInParent<Damageable>();
            
            if(!_damageable) Debug.LogError("DamagePostProcessFilter couldn't find Damageable component in parent, will likely result in errors.");
        }
    }
}