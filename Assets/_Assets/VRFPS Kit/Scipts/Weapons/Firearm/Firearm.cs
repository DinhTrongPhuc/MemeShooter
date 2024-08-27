using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

#if HAPTIC_PATTERNS
using HapticPatterns;
#endif

namespace VRFPSKit
{
    /// <summary>
    /// Main script that handles behaviour which is in common for all firearms,
    /// the rest of the components are compositional and added when needed
    /// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    public class Firearm : MonoBehaviour
    {
        #if HAPTIC_PATTERNS
        public HapticPattern shootHaptic;
        #else
        [Header("VR Haptic Patterns Isn't Installed")]
        #endif
        
        [Space] 
        public FireMode[] availableFireModes;
        [Space] 
        
        [Header("Current State")]
        public Cartridge chamberCartridge;
        public Magazine magazine;
        public FireMode currentFireMode;
        [HideInInspector] public bool isActionOpen;
        [HideInInspector] public string weaponName;
        //TODO hammer state
        
        public event Action<Cartridge> ShootEvent;

        public void TryShoot()
        {
            //Can't shoot unless action is in battery
            if (isActionOpen) return;
            
            //Make sure the cartridge can fire
            if (!chamberCartridge.CanFire()) return;
            
            Cartridge unspentCartridge = chamberCartridge;
            chamberCartridge.Consume();
            
            //Fire if all checks pass
            ShootEvent?.Invoke(unspentCartridge);
            
            //If haptic patterns is installed, play shoot haptic
            #if HAPTIC_PATTERNS 
            if (shootHaptic != null)
                shootHaptic.PlayOverTime(GetComponent<XRGrabInteractable>());
            #endif
        }

        /// <summary>
        /// If a cartridge is in the chamber, eject it
        /// </summary>
        public void TryEjectChamber()
        {
            //Cant eject empty chamber
            if (chamberCartridge.IsNull())
                return;

            //If an ejector exists, visually eject the round
            GetComponent<CartridgeEjector>()?.EjectCartridge(chamberCartridge);

            //Chamber is emptied
            chamberCartridge = Cartridge.Empty;
        }
        
        /// <summary>
        /// If possible, load chamber from magazine
        /// </summary>
        public void TryLoadChamber()
        {
            //Cant load filled chamber
            if (!chamberCartridge.IsNull())
                return;
            //Cant load with empty mag
            if (magazine == null || magazine.IsEmpty())
                return;

            chamberCartridge = magazine.GetTopCartridge();
            magazine.RemoveCartridgeFromTop();
        }

        /// <summary>
        /// Called when saving / building in the editor
        /// </summary>
        protected void OnValidate()
        {
            //PrefabUtility.GetCorrespondingObjectFromOriginalSource() is only available in the editor
            #if UNITY_EDITOR
                //Store weapon name based on the original prefab name
                GameObject prefabObj = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
                if(prefabObj)
                    weaponName = prefabObj.name;
            #else
                //Throw error if weaponName for some reason is empty
                if(weaponName == String.Empty)
                    Debug.LogError($"Weapon.weaponName has not been assigned in '{gameObject.name}', this is automatically set when you manually save the prefab");
            #endif
        }
    }
}