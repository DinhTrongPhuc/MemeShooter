using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine;
using UnityEngine.Serialization;

namespace VRFPSKit
{
	/// <summary>
	/// Component lets player grab with two hands and scale the object by pulling it
	/// </summary>
    [RequireComponent(typeof(XRGrabInteractable))]
    public class XRDoubleHandScaler : MonoBehaviour
    {
	    public float minSizeRatio = 1;
	    public float maxSizeRatio = 1;
	    [Space]
        public bool resetSize = true;
		
		private Vector3 _interactionStartSize;
		private float _interactionStartHandDistance;
		private bool _twoHandsLastFrame;
		
        private XRGrabInteractable _grabbable;

        // Update is called once per frame
        void Update()
        {
	        //Detect if this is the first frame without two hands
            if (!TwoHandsHolding() && _twoHandsLastFrame)
            {
	            if(resetSize)
					transform.localScale = _interactionStartSize;
            }
            
			//Detect if this is the first frame with two hands
			if(TwoHandsHolding() && !_twoHandsLastFrame)
			{
            	_interactionStartSize = transform.localScale;
				_interactionStartHandDistance = DistanceBetweenHands();
			}

			TryScaleByHandDistance();
        }

        private void TryScaleByHandDistance()
        {
	        if (!TwoHandsHolding())
		        return;
	        
	        transform.localScale = _interactionStartSize * GetDistanceScaledRatio();
        }

		public float GetDistanceScaledRatio()
		{
			//Make sure values are initialized
			if (!TwoHandsHolding() || !_twoHandsLastFrame)
				return 1;
			
			float deltaDistanceRatio = DistanceBetweenHands() / _interactionStartHandDistance;
			deltaDistanceRatio = Mathf.Clamp(deltaDistanceRatio, minSizeRatio, maxSizeRatio);

			return deltaDistanceRatio;
		}

		private bool TwoHandsHolding()
		{	
			return _grabbable.interactorsSelecting.Count > 1;
		}

		private float DistanceBetweenHands()
		{	
			if (!TwoHandsHolding())
				return 0;

			return Vector3.Distance(
				_grabbable.interactorsSelecting[0].transform.position, 
				_grabbable.interactorsSelecting[1].transform.position);
		}
		
		private void LateUpdate()
		{
			_twoHandsLastFrame = TwoHandsHolding();
		}
		
		void Awake()
		{
			_grabbable = GetComponent<XRGrabInteractable>();
			
			if (_grabbable.selectMode != InteractableSelectMode.Multiple)
				Debug.LogWarning("XRDoubleHandScaler won't be able to scale unless select mode is set to Multiple");

			if (_grabbable.trackScale)
			{
				_grabbable.trackScale = false;
				Debug.LogWarning("XRDoubleHandScaler won't be able to scale if XRGrabInteractable.trackScale is enabled, changing automatically");
			}
		}
    }
}