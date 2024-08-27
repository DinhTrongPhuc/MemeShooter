using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [CreateAssetMenu(fileName = "Ballistic Profile", menuName = "Ballistic Profile")]
    public class BallisticProfile : ScriptableObject
    {
        public int projectileAmount = 1;
        [Space]
        [Header("Projectile Properties")]
        public float baseDamage = 50;
        [Space]
        [Space]
        [Header("Projectile Trajectory")]
        public float startVelocity = 500;
        public float gravityScale = 1;
        public float randomSpreadAngle = 0;

    }
}
