using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class PlayerCollision : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<PracticeBullet>()) {
                GetComponent<PlayerHealth>().GotHit(10);
            }
        }
    }
}