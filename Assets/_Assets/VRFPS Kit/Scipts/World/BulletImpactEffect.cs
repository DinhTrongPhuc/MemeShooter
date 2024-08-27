using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    /// <summary>
    /// Can be applied to any object with a collider to change the Bullet Hit Particle Effect.
    /// Bullet.cs looks for this script when it collides, but it does nothing on it's own.
    /// </summary>
    public class BulletImpactEffect : MonoBehaviour
    {
        public GameObject impactEffect;

        /// <summary>
        /// This method has to be here, it is a quirk with unity that you can't disable scripts
        /// Without a Start()/Update() method. We want to be able to toggle the HitParticleEffect
        /// </summary>
        private void Start() { }
    }

}
