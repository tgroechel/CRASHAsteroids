using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crash {
    public class KuriManager : Singleton<KuriManager> {
        Rigidbody rb;
        public static float MAX_SPEED = 1.0f, MASS = 3.0f, ANGLE_EPSILON = 5.0f, KDP = 0.2f;
        public float KDD = 0.01f, KDI = 0.01f;
        public float lastError, i_error;
        public float desiredVelZ, desiredRotation;
        Vector3 forceToApply, rotationToApply;
        void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                // PublishVelocityCommand(desiredVelZ, desiredRotation);
            }
        }

        public void PublishVelocityCommand(float linear, float rotationAngle) {
            desiredVelZ = linear;
            desiredRotation = rotationAngle;
        }

        public void SetVelocity(float linear) {
            desiredVelZ = linear;
        }

        private void FixedUpdate() {
            if (Mathf.Abs(rb.velocity.z) <= Mathf.Abs(desiredVelZ)) {
                forceToApply.z = desiredVelZ * MASS;
                rb.AddForce(forceToApply, ForceMode.VelocityChange);
            }

            if (desiredRotation < transform.rotation.eulerAngles.y - ANGLE_EPSILON ||
                desiredRotation > transform.rotation.eulerAngles.y + ANGLE_EPSILON) {
                float p_error = desiredRotation - transform.rotation.eulerAngles.y;
                float d_error = p_error - lastError;
                i_error += (p_error * Time.fixedDeltaTime);
                lastError = p_error;
                Debug.Log(d_error);
                rotationToApply.y = KDP * p_error + KDD * d_error + KDI * i_error;
                rb.AddTorque(rotationToApply, ForceMode.VelocityChange);
            }
            else {
                rotationToApply.y = 0;
                lastError = 0;
                i_error = 0;
                rb.AddTorque(rotationToApply, ForceMode.VelocityChange);
            }
        }


    }
}
