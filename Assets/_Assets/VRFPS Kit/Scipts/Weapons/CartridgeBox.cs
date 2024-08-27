using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(XRDoubleHandScaler))]
    public class CartridgeBox : MonoBehaviour
    {
        public Cartridge cartridgeType;
        public int cartridgeAmount = 20;
        public float cartridgeSpawnRadius = .075f;
        [Space]
        public GameObject cartridgePrefab;

        private XRDoubleHandScaler _scaler;
        private bool _hasBeenOpened;
        

        // Update is called once per frame
        void Update()
        {
            //Wait until box has been maximally opened/scaled by player
            if(!Mathf.Approximately(_scaler.GetDistanceScaledRatio(), _scaler.maxSizeRatio)) return;
            if (_hasBeenOpened) return;
            
            _hasBeenOpened = true;
            OpenBox();
        }

        private void OpenBox()
        {
            //Spawn x amounts of cartridges at random position
            for (int i = 0; i < cartridgeAmount; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(-cartridgeSpawnRadius, cartridgeSpawnRadius), 
                    Random.Range(-cartridgeSpawnRadius, cartridgeSpawnRadius), 
                    Random.Range(-cartridgeSpawnRadius, cartridgeSpawnRadius));
                
                GameObject cartridgeObj = Instantiate(
                    cartridgePrefab, 
                    transform.position + randomPosition,
                    Quaternion.identity);
                
                CartridgeItem cartridgeItem = cartridgeObj.GetComponent<CartridgeItem>();
                cartridgeItem.cartridge = cartridgeType;
            }
            
            //Destroy the box when it has been used
            Destroy(gameObject);
        }
        
        void Awake()
        {
            _scaler = GetComponent<XRDoubleHandScaler>();
        }
    }
}
