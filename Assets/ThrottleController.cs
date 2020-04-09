using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crash {
    public class ThrottleController : MonoBehaviour, IMixedRealityPointerHandler {
        public const float MAX_ANGLE = 130, RADIUS = 1.2f;
        LineRenderer lr;
        TextMeshProUGUI textMesh;

        void Start() {
            lr = GetComponent<LineRenderer>();
            lr.SetPosition(0, transform.parent.position);
            lr.SetPosition(1, transform.position);
            textMesh = transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        }

        void Update() {
            lr.SetPosition(0, transform.parent.position);
            lr.SetPosition(1, transform.position);
        }

        public Vector3 ProjectPointToSphere(Vector3 point, Vector3 center, float radius) {
            return center + radius * (point - center).normalized;
        }

        public Vector3 ProjectPointToCircle(Vector3 point, Vector3 center, Vector3 circleNormal, float radius) {
            return ProjectPointToSphere(Vector3.ProjectOnPlane(point, circleNormal), center, radius);
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData) {
            Vector3 pointerPos = eventData.Pointer.Position;
            Vector3 center = Vector3.zero;
            Vector3 norm = transform.parent.right;

            Vector3 potentialPosition = ProjectPointToCircle(pointerPos, center, norm, RADIUS);
            float angleFromOutward = Vector3.SignedAngle(transform.parent.forward, potentialPosition, norm);

            // -120 up, 120 down
            float percent = 0;
            if (angleFromOutward < 0) {
                if (Mathf.Abs(angleFromOutward) < MAX_ANGLE) {
                    potentialPosition = (Quaternion.Euler(-MAX_ANGLE, 0, 0) * transform.parent.forward).normalized * RADIUS;
                    angleFromOutward = -MAX_ANGLE;
                }
                percent = (180 + angleFromOutward) / (180 - MAX_ANGLE);
            }
            else {
                if (Mathf.Abs(angleFromOutward) < MAX_ANGLE) {
                    potentialPosition = (Quaternion.Euler(MAX_ANGLE, 0, 0) * transform.parent.forward).normalized * RADIUS;
                    angleFromOutward = MAX_ANGLE;
                }
                percent = -(180 - angleFromOutward) / (180 - MAX_ANGLE);
            }

            transform.localPosition = potentialPosition;
            KuriManager.instance.SetVelocity(percent);
            textMesh.SetText(percent.ToString("#.00"));
        }

        // Needed for IMixedRealityPointerHandler
        public void OnPointerUp(MixedRealityPointerEventData eventData) {
        }
        public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        }

        public void OnPointerDown(MixedRealityPointerEventData eventData) {
        }
    }
}
