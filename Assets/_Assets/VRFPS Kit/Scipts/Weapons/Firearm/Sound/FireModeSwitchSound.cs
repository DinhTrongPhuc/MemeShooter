using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    [RequireComponent(typeof(Firearm))]
    public class FireModeSwitchSound : MonoBehaviour
    {
        public AudioSource switchSound;

        private FireMode _fireModeLastFrame;

        private Firearm _firearm;

        private void Update()
        {
            //If fire mode changed this frame, play sound
            if(_fireModeLastFrame != _firearm.currentFireMode)
                switchSound.Play();
            
            _fireModeLastFrame = _firearm.currentFireMode;
        }

        private void Awake()
        {
            _firearm = GetComponent<Firearm>();
        }
    }
}
