using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    public class DamageableHealthReset : MonoBehaviour
    {
        public GameObject[] damageableContainers;

        public void ResetHealth()
        {
            foreach (GameObject damageableContainer in damageableContainers)
            {
                foreach (Damageable damageable in damageableContainer.GetComponentsInChildren<Damageable>())
                    damageable.ResetHealth();
            }
        }
    }
}