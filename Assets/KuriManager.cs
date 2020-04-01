using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crash {
    public class KuriManager : Singleton<KuriManager> {
        Rigidbody rb;
        public static float MAX_SPEED = 1.0f, MASS = 100.0f, ANGLE_EPSILON = 5.0f;
        KuriWheelController kuriWheelController;

        void Awake() {
            rb = GetComponent<Rigidbody>();
            kuriWheelController = GetComponent<KuriWheelController>();
        }

        internal void AlertHeadPat() {
            Debug.Log("Head was pat");
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SetVelocity(1);
            }
        }

        public void SetVelocity(float linear) {

        }



    }
}
