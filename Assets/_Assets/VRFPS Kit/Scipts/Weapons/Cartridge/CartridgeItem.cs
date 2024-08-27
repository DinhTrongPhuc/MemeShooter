using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    /// <summary>
    /// Represents a physical Cartridge Object in the world
    /// </summary>
    public class CartridgeItem : MonoBehaviour
    {
        public Cartridge cartridge;

        public CartridgeRenderer cartridgeRenderer;

        // Start is called before the first frame update
        void Start()
        {
            if (cartridgeRenderer == null)
                Debug.LogError("CartridgeItem renderer is not assigned");
        }

        // Update is called once per frame
        void Update()
        {
            cartridgeRenderer.cartridgeToRender = cartridge;
        }
    }
}