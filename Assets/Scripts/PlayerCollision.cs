using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class PlayerCollision : MonoBehaviour {

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.GetComponent<ProjectileMoveScript2>()) {
                GetComponent<PlayerHealth>().GotHit(10);
            }
        }
    }
}