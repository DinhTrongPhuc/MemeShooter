using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Modifies render visibility status and renderer material to match with cartridgeToRender
    /// </summary>
    public class CartridgeRenderer : MonoBehaviour
    {
        public Cartridge cartridgeToRender;
        [Space] 
        public Renderer casingRenderer;
        public Renderer bulletRenderer;
        public Renderer emptyRenderer;
        [Space] 
        public BulletTypeMaterial[] bulletTypeMaterials;

        private Cartridge _cartridgeLastFrame;
        
        // Update is called once per frame
        void Update()
        {
            if (cartridgeToRender.Equals(_cartridgeLastFrame)) return;
            
            UpdateRenderer();
            _cartridgeLastFrame = cartridgeToRender;
        }

        private void Start()
        {
            UpdateRenderer();
        }

        public void UpdateRenderer()
        {
            //Hide renders if we aren't supposed to render anything
            bool visibleModel = cartridgeToRender.caliber != Caliber.None;
            bool renderBullet = cartridgeToRender.bulletType != BulletType.Empty_Casing;
            
            if(casingRenderer) casingRenderer.enabled = visibleModel;
            if(bulletRenderer) bulletRenderer.enabled = visibleModel && renderBullet;
            if(emptyRenderer) emptyRenderer.enabled = visibleModel && !renderBullet;

            Material bulletMaterial = GetBulletMaterial();
            if(bulletMaterial != null)
                bulletRenderer.material = bulletMaterial;
        }

        private Material GetBulletMaterial()
        {
            foreach (var bulletMaterial in bulletTypeMaterials)
                //Return bullet material if active bullet type matches
                if (cartridgeToRender.bulletType == bulletMaterial.bulletType)
                    return bulletMaterial.bulletMaterial;

            return null;
        }
    }
    
    [Serializable]
    public struct BulletTypeMaterial
    {
        public BulletType bulletType;
        public Material bulletMaterial;
    }
}