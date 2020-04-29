using UnityEngine;

namespace Crash {
    public class KuriManager : Singleton<KuriManager> {
        Rigidbody rb;
        public const float MAX_SPEED = 1.0f, MASS = 100.0f, ANGLE_EPSILON = 2.0f, MAX_TURN = 360, MIN_TURN = 0;
        KuriWheelController kuriWheelController;

        [Range(-MAX_SPEED, MAX_SPEED)]
        public float desiredSpeed;

        [Range(MIN_TURN, MAX_TURN)]
        public float desiredRotation;

        public float turnSpeed = 3.0f;

        public GameObject shield;

        void Awake() {
            rb = GetComponent<Rigidbody>();
            kuriWheelController = GetComponent<KuriWheelController>();
            shield.SetActive(false);
        }

        public void AlertHeadPat() {
            shield.SetActive(!shield.activeSelf);
        }

        public void SetVelocity(float linear) {
            if (linear >= 0) {
                desiredSpeed = Mathf.Min(linear, MAX_SPEED);
            }
            else {
                desiredSpeed = Mathf.Max(linear, -MAX_SPEED);
            }
        }

        public void SetRotation(float rotation) {
            desiredRotation = rotation;
        }

        private void FixedUpdate() {
            UpdateVelocity();
            Turn();
        }

        private void UpdateVelocity() {
            transform.position = transform.position + transform.forward * desiredSpeed * Time.deltaTime;
        }

        private void Turn() {
            float error = ((desiredRotation + transform.rotation.eulerAngles.y) % 360) - 180;
            float turnAngle = error > 0 ? -turnSpeed : turnSpeed;
            if (Mathf.Abs(error) > ANGLE_EPSILON) {
                transform.Rotate(Vector3.up, turnAngle, Space.Self);
            }

        }
    }
}
