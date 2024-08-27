using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VRFPSKit.Input;

namespace VRFPSKit
{
    [RequireComponent(typeof(XRGrabInteractable), typeof(Magazine))]
    public class MagazineUnloader : MonoBehaviour
    {
        public XRHeldInputAction unloadInput;
        public AudioSource unloadSound;
        public Transform unloadPosition;
        public GameObject cartridgePrefab;
        [HideInInspector] public float lastUnloadTime;

        private Magazine _magazine;
        private XRGrabInteractable _interactable;

        // Update is called once per frame
        private void Update()
        {
            TryUnloadCartridge();
        }

        private void TryUnloadCartridge()
        {
            if (!_interactable.isSelected) return;
            if (_magazine.IsEmpty()) return;
            
            //Get input for current holding hand
            InputAction input = unloadInput.GetActionForPrimaryHand(_interactable);
            
            if(input == null) return;
            if(!input.WasPressedThisFrame()) return;

            Cartridge topCartridge = _magazine.GetTopCartridge();
            
            //All checks were passed, unload one cartridge
            _magazine.RemoveCartridgeFromTop();
            SpawnCartridge(topCartridge);
            
            lastUnloadTime = Time.time;
        }
        
        private void SpawnCartridge(Cartridge ejectedCartridge)
        {
            GameObject cartridgeObject = Instantiate(cartridgePrefab, unloadPosition.position, unloadPosition.rotation);
            
            CartridgeItem cartridgeItem = cartridgeObject.GetComponent<CartridgeItem>();
            cartridgeItem.cartridge = ejectedCartridge;
            
            unloadSound.Play();
        }
        
        // Start is called before the first frame update
        void Awake()
        {
            _magazine = GetComponent<Magazine>();
            _interactable = GetComponent<XRGrabInteractable>();
        }
    }
}