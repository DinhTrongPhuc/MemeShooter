using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRFPSKit
{
    public class DamageableHealthTextUI : MonoBehaviour
    {
        public TMP_Text text;
        public Damageable damageable;

        // Update is called once per frame
        void Update()
        {
            int health = (int)damageable.health;
            
            if(health > 0)
                text.text = health + " HP";
            else 
                text.text = "Dead";
        }
    }
}