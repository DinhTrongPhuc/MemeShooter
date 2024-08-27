using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Makes the Cartridge Renderer render the cartridge that is in Firearm Chamber
    /// </summary>
    [RequireComponent(typeof(CartridgeRenderer))]
    public class ChamberCartridgeRenderer : MonoBehaviour
    {
        private Firearm _firearm;
        private CartridgeRenderer _cartridgeRenderer;

        private void Awake()
        {
            _cartridgeRenderer = GetComponent<CartridgeRenderer>();
            _firearm = GetComponentInParent<Firearm>();
            if (_firearm == null)
                Debug.LogError("No parent Weapon component was found");
        }

        private void Update()
        {
            _cartridgeRenderer.cartridgeToRender = _firearm.chamberCartridge;
        }
    }
}