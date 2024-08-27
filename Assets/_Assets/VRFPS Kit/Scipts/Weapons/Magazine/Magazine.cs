using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Representing a magazine that stores cartridges. Can be used on a magazine item
    /// object or as an internal magazine by attaching it to a firearm
    /// </summary>
    public class Magazine : MonoBehaviour
    {
        public Caliber caliber;
        public int capacity;

        public Firearm[] compatibleWeaponPrefabs;
        
        [Space]
        //TODO let inspector change magazine contents
        public readonly List<Cartridge> cartridges = new();
        
        
        private void Start()
        {
            AddCartridgeToTop(new Cartridge(caliber, BulletType.FMJ), capacity);
        }

        public Cartridge GetTopCartridge()
        {
            if (cartridges.Count == 0)
                return Cartridge.Empty;

            return cartridges[^1];
        }

        public void AddCartridgeToTop(Cartridge cartridge, int amount = 1)
        {
            //Add cartridge to top x times
            for (int i = 0; i < amount; i++)
            {
                //Check if cartridge fits
                if (IsFull())
                    return;

                cartridges.Add(cartridge);
            }
        }

        public void RemoveCartridgeFromTop(int amount = 1)
        {
            //Remove cartridge from top x times
            for (int i = 0; i < amount; i++)
            {
                int index = cartridges.Count - 1;

                if (index < 0)
                    break;
                
                cartridges.RemoveAt(index);
            }
        }

        public bool IsEmpty()
        {
            return cartridges.Count == 0;
        }

        public bool IsFull()
        {
            return cartridges.Count == capacity;
        }

        /// <summary>
        /// Determines whether compatibleWeaponPrefabs contains the specified Weapon, and thus is compatible
        /// </summary>
        /// <param name="firearm">Firearm to check compatibility with</param>
        /// <returns>Is compatible?</returns>
        public bool IsCompatibleWithWeapon(Firearm firearm){
            //TODO prefab name comparison to determine compatibility is unreliable
            foreach(Firearm compatibleWeapon in compatibleWeaponPrefabs)
                if(firearm.weaponName.Equals(compatibleWeapon.weaponName))
                    return true;
                     
            return false;
        }
    }
}