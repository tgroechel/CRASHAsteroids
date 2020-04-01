using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class KuriWheelController : MonoBehaviour {
        private float m_horizontalInput;
        private float m_verticalInput;
        private float m_steeringAngle;

        public WheelCollider frontLeftWheel, frontRightWheel;
        public WheelCollider rearLeftWheel, rearRightWheel;
        public float desiredAngle = 0;

        public float motorForce = 50;


        public void GetInput() {
            m_horizontalInput = Input.GetAxis("Horizontal");
            m_verticalInput = Input.GetAxis("Vertical");
        }

        private void Steer() {
            // check angle vs current angle
            float error = desiredAngle - transform.rotation.eulerAngles.y;
            float addedforce = m_horizontalInput * motorForce;
            if (error > 180) { // turn left
                frontLeftWheel.motorTorque += addedforce;

            }
            else { // turn right
                frontRightWheel.motorTorque += m_horizontalInput * motorForce;
            }
        }

        private void Accelerate() {
            frontLeftWheel.motorTorque = m_verticalInput * motorForce;
            frontRightWheel.motorTorque = m_verticalInput * motorForce;
        }

        private void FixedUpdate() {
            GetInput();
            Accelerate();
            Steer();
        }


    }
}