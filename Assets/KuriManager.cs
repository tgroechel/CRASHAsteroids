using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Crash {
    public class KuriManager : Singleton<KuriManager> {
        Rigidbody rb;
        public static float MAX_SPEED = 1.0f, MASS = 3.0f, ANGLE_EPSILON = 2.0f;
        public float desiredVelZ;
        public float desiredRotation;
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

        private void FixedUpdate() {
            rb.inertiaTensor = rb.inertiaTensor;

            rb.inertiaTensorRotation = rb.inertiaTensorRotation;
            if (Mathf.Abs(rb.velocity.z) <= Mathf.Abs(desiredVelZ)) {
                forceToApply.z = desiredVelZ * MASS;
                rb.AddForce(forceToApply, ForceMode.VelocityChange);
            }

            if (desiredRotation < transform.rotation.eulerAngles.y - ANGLE_EPSILON ||
                desiredRotation > transform.rotation.eulerAngles.y + ANGLE_EPSILON) {
                //rotationToApply.y = 1;
                rb.AddTorque(rotationToApply, ForceMode.VelocityChange);
            }
            else {
                rotationToApply.y = 0;
                rb.AddTorque(rotationToApply, ForceMode.VelocityChange);
            }
        }


    }
}
