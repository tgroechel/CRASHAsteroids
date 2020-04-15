using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class TempBulletShooter : Singleton<TempBulletShooter> {
        GameObject pracBullet;
        public float speed = 5f;
        private void Awake() {
            pracBullet = transform.GetChild(0).gameObject;
        }

        public void ResetBullet() {
            pracBullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pracBullet.transform.localPosition = Vector3.zero;
        }

        public void ShootAtPlayer() {
            ResetBullet();
            pracBullet.transform.LookAt(Camera.main.transform);
            pracBullet.GetComponent<Rigidbody>().velocity = speed * pracBullet.transform.forward;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Y)) {
                ShootAtPlayer();
            }
        }

    }
}
