using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace VRFPSKit
{
    /// <summary>
    /// DEPRECATED, behaviour has been moved to BulletShooter.cs
    /// </summary>
    [Obsolete("ShotgunPelletShooter behaviour was moved to the BulletShooter in version v4.0, this script will be removed in a future update")]
    public class ShotgunPelletShooter : BulletShooter
    {
        private void Awake()
        {
            Debug.LogError("ShotgunPelletShooter has been deprecated, use BulletShooter instead");
        }
    }
}
