using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Makes the Cartridge Renderer render a cartridge from the parent magazine according to our sibling index
    /// </summary>
    [RequireComponent(typeof(CartridgeRenderer))]
    public class MagazineCartridgeRenderer : MonoBehaviour
    {
        private int _siblingIndex;
        
        private Magazine _magazine;
        private CartridgeRenderer _cartridgeRenderer;

        private void Update()
        {
            _cartridgeRenderer.cartridgeToRender = GetCartridgeToRender();
        }

        private Cartridge GetCartridgeToRender()
        {
            //Start at the top cartridge, and then work back with every sibling index
            //This way, you can remove cartridges from the bottom (for performance perhaps) and still retain working indexes
            int cartridgeIndex = (_magazine.cartridges.Count - 1) - _siblingIndex;
            
            //Don't render if index is outside list
            if (cartridgeIndex < 0)
                return Cartridge.Empty;
            
            return _magazine.cartridges[cartridgeIndex];
        }
        
        private void Awake()
        {
            //Cache sibling index for performance
            _siblingIndex = transform.GetSiblingIndex();
            
            _cartridgeRenderer = GetComponent<CartridgeRenderer>();
            _magazine = GetComponentInParent<Magazine>();
            
            if (!_magazine)
                Debug.LogError("No parent Magazine component was found");
        }
    }
}