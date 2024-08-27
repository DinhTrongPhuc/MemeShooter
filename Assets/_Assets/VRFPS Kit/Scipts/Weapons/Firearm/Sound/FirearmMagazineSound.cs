using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    [RequireComponent(typeof(Firearm))]
    public class FirearmMagazineSound : MonoBehaviour
    {
        public AudioSource magazineSound;

        private Magazine _magazineLastFrame;

        private Firearm _firearm;

        private void Update()
        {
            //If magazine changed this frame, play sound
            if(_magazineLastFrame != _firearm.magazine)
                magazineSound.Play();
            
            _magazineLastFrame = _firearm.magazine;
        }
        
        private void Awake()
        {
            _firearm = GetComponent<Firearm>();
        }
    }
}
