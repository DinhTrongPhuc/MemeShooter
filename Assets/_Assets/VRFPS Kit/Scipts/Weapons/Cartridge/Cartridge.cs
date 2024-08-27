using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Struct that tracks all cartridge properties
    /// </summary>
    [Serializable]
    public struct Cartridge
    {
        public static Cartridge Empty => new Cartridge(Caliber.None, BulletType.Empty_Casing);

        public Caliber caliber;
        public BulletType bulletType;

        public Cartridge(Caliber caliber, BulletType bulletType)
        {
            this.caliber = caliber;
            this.bulletType = bulletType;
        }
        
        public void Consume()
        {
            bulletType = BulletType.Empty_Casing;
        }

        public bool IsNull()
        {
            return caliber == Caliber.None;
        }

        public bool CanFire()
        {
            return bulletType != BulletType.Empty_Casing;
        }
    }
}