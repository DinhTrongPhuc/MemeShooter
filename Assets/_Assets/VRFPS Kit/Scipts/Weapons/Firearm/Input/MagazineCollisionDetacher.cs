using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VRFPSKit
{
    /// <summary>
    /// Detaches Currently attached magazine when a new magazine enters trigger.
    /// Perfect for AK Magazines, which can be quickly detached by hitting them in real life.
    /// </summary>
    public class MagazineCollisionDetacher : MonoBehaviour
    {
        public Vector3 detachVelocity;
        
        private MagazineInteractor _magazineInteractor;

        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            //If magazine Interactor isn't holding a magazine, there is nothing to detach, return
            if (_magazineInteractor.interactablesSelected.Count == 0) return;
            
            Magazine attachedMagazine = _magazineInteractor.GetAttachedMagazine();
            Magazine collidingMagazine = other.GetComponentInParent<Magazine>();
            
            //If colliding object isn't a magazine, return
            if (!collidingMagazine) return;
            //if attached magazine triggers deattach on itself, return
            if (attachedMagazine == collidingMagazine) return;
            
            _magazineInteractor.DetachMagazine();

            Vector3 localDetachVelocity = transform.TransformVector(detachVelocity);
            attachedMagazine.GetComponentInParent<Rigidbody>().AddForce(localDetachVelocity, ForceMode.VelocityChange);
        }

        // Start is called before the first frame update
        void Awake()
        {
            _magazineInteractor = GetComponentInParent<MagazineInteractor>();
            
            if(!_magazineInteractor) Debug.LogError("No MagazineInteractor was found in parent, component won't work");

            if (Physics.GetIgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Interactable")))
                Debug.LogError($"{gameObject.name} is on a layer that cant't collide with Magazines, MagazineCollisionDetacher won't work");
        }
    }
}
