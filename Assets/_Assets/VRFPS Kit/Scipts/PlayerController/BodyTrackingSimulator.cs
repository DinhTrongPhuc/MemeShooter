using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    /// <summary>
    /// Body transform simply tracks head movement
    /// </summary>
    public class BodyTrackingSimulator : MonoBehaviour
    {
        public Transform headTransform;
        public Vector3 headPositionOffset;

        // Update is called once per frame
        void Update()
        {
            //Mimic head position with offset
            transform.position = headTransform.position + headPositionOffset;

            //Track only y rotation of the head
            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                headTransform.rotation.eulerAngles.y, 
                transform.rotation.eulerAngles.z);
        }
    }
}