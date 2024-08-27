using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemeShooter
{
    public class MemeGo : MonoBehaviour
    {
        public MemeDamageHandler memeDamageHandler;

        public SpriteRenderer memeAvatar;

        [SerializeField]
        int score = 10;

        void Awake()
        {
            memeDamageHandler.OnSetupEvent.AddListener(GetRandomAvatar);

            memeDamageHandler.OnDeadEvent.AddListener(AddScore);
        }

        private void Update()
        {
            LookAt(Camera.main.transform.position, memeAvatar.transform);
        }

        void GetRandomAvatar()
        {
            memeAvatar.sprite = MemeSpawner.Instance.GetRandomAvatar();
        }

        void AddScore()
        {
            Debug.Log("Addscore " + score);
            Debug.Log("Add kill " + memeAvatar.sprite.name);

            MemeGameController.Instance.AddScore(score);
            MemeGameController.Instance.AddKilled(memeAvatar.sprite.name);
        }

        public void LookAt(Vector3 worldLookPos, Transform rotTranfrom)
        {
            Vector3 relativePos = worldLookPos - rotTranfrom.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

            Vector3 euler = rotation.eulerAngles;

            euler.z = 0f;

            euler.x = 0f;

            rotation.eulerAngles = euler;

            rotTranfrom.rotation = rotation;
        }
    }
}
