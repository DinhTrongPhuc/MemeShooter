using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
    public class SkeetSpawner : MonoBehaviour
    {
        public Transform[] spawnPoints;
        public float spawnInterval;
        public GameObject skeetPrefab;
    
        // Start is called before the first frame update
        void Start()
        {
            InvokeRepeating(nameof(TryLaunchSkeet), 0, spawnInterval);
        }

        public void TryLaunchSkeet()
        {
			if (!enabled) return;
            if (spawnPoints.Length == 0) return;
            
            //Randomize a spawn point
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject skeet = Instantiate(skeetPrefab, spawn.position, spawn.rotation);
            //TODO sound
        }
    }
}
