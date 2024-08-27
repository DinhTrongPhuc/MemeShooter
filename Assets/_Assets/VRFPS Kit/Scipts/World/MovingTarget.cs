using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRFPSKit
{
    public class MovingTarget : MonoBehaviour
    {
        public Vector3 travelTo;
        public float speed;

        private Vector3 _startPosition;
        private bool _returnTravel;
        private float _lastSwitchDirectionTime;

        // Start is called before the first frame update
        void Start()
        {
            _startPosition = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            TravelToTarget();
            CalculateSwitchDirection();
        }

        private void TravelToTarget()
        {
            Vector3 moveDelta = Vector3.MoveTowards(
                transform.localPosition,
                GetCurrentTarget(),
                speed * Time.deltaTime);

            transform.localPosition = moveDelta;
        }

        private void CalculateSwitchDirection()
        {
            if (Time.time - _lastSwitchDirectionTime <= .5f)
                return;
            if (Vector3.Distance(transform.localPosition, GetCurrentTarget()) > .1f)
                return;

            _returnTravel = !_returnTravel;
            _lastSwitchDirectionTime = Time.time;
        }

        private Vector3 GetCurrentTarget()
        {
            if (_returnTravel)
                return _startPosition;

            return travelTo;
        }

        void OnDrawGizmos()
        {
            // Draws a red line from this transform to the target
            Vector3 offset = new Vector3(0, .2f, 0);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + offset, transform.parent.TransformPoint(travelTo) + offset);
        }
    }
}