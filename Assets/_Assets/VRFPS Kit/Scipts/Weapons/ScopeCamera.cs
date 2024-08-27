using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    /// <summary>
    /// Makes sure the scope camera is only rendering when held,
    /// and makes sure no more than one camera can render at the same time
    /// </summary>
    [RequireComponent(typeof(Camera), typeof(BoxCollider))]
    public class ScopeCamera : MonoBehaviour
    {
        private Camera _playerCamera;
        private Camera _scopeCamera;
        private BoxCollider _trigger;

        private void Update()
        {
            //we can't determine wether we should render without the playerCamera
            if (!_playerCamera) return;

            bool playerCameraIsInRenderBounds = _trigger.bounds.Contains(_playerCamera.transform.position);

            _scopeCamera.enabled = playerCameraIsInRenderBounds;
        }

        private void UpdateMainCamera()
        {
            _playerCamera = Camera.main;
        }

        void Awake()
        {
            _scopeCamera = GetComponent<Camera>();
            _trigger = GetComponent<BoxCollider>();
            _scopeCamera.enabled = false;
            
            UpdateMainCamera();
            //This is a pretty performance heavy method, so we don't run it in update
            //In case the main camera should change, we still need to update it every once in a while
            InvokeRepeating(nameof(UpdateMainCamera), 0, 2);
        }
    }
}
