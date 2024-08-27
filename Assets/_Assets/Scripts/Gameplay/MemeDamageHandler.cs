using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using VRFPSKit;


namespace MemeShooter
{
    [RequireComponent(typeof(Damageable))]
    public class MemeDamageHandler : MonoBehaviour
    {
        //public float Life = 10f;

        float currentLife;

        public Rigidbody Rigidbody { get; private set; }

        public UnityEvent OnSetupEvent = new();

        public UnityEvent OnDeadEvent = new();

        private void Start()
        {
            Setup();
        }

        public void Setup()
        {
            currentLife = GetComponent<Damageable>().health;

            Rigidbody = GetComponent<Rigidbody>();

            if (OnSetupEvent != null)
            {
                OnSetupEvent.Invoke();
            }
        }

        void Update()
        {

            currentLife = GetComponent<Damageable>().health;

            if (currentLife <= 0)
            {
                if (OnDeadEvent != null)
                {
                    OnDeadEvent.Invoke();
                }

                GetComponent<Damageable>().ResetHealth();

                gameObject.SetActive(false);
            }
        }

        //public virtual void TakeDamage(float damage)
        //{
        //    currentLife -= damage;

        //    if (currentLife <= 0)
        //    {
        //        if (OnDeadEvent != null)
        //        {
        //            OnDeadEvent.Invoke();
        //        }

        //        gameObject.SetActive(false);
        //    }


        //public override void HandleDamageProvider(HVRDamageProvider damageProvider, Vector3 hitPoint, Vector3 direction)
        //{
        //    base.HandleDamageProvider(damageProvider, hitPoint, direction);

        //    if (Rigidbody)
        //    {
        //        Rigidbody.AddForceAtPosition(direction.normalized * damageProvider.Force, hitPoint, ForceMode.Impulse);
        //    }

        //    //Debug.Log("GO " + gameObject.name + " Hit");
        //}
    }

    
}
