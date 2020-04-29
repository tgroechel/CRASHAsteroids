using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crash {
    public class ThrottleController : MonoBehaviour, IMixedRealityPointerHandler {
        public const float MAX_ANGLE = 25, RADIUS = 0.2f;
        public LineRenderer lr;
        TextMeshProUGUI textMesh;
        PointerData curPointer;
        public Transform relativeTransform;

        private struct PointerData {
            public IMixedRealityPointer pointer;
            private Vector3 initialGrabPointInPointer;

            public PointerData(IMixedRealityPointer pointer, Vector3 initialGrabPointInPointer) : this() {
                this.pointer = pointer;
                this.initialGrabPointInPointer = initialGrabPointInPointer;
            }

            public bool IsNearPointer() {
                return (pointer is IMixedRealityNearPointer);
            }

            /// Returns the grab point on the manipulated object in world space
            public Vector3 GrabPoint {
                get {
                    return (pointer.Rotation * initialGrabPointInPointer) + pointer.Position;
                }
            }
        }

        void Start() {
            //lr = GetComponentInChildren<LineRenderer>();
            lr.SetPosition(0, relativeTransform.position);
            lr.SetPosition(1, transform.position);
            textMesh = transform.GetComponentInChildren<Canvas>().GetComponentInChildren<TextMeshProUGUI>();
        }

        void Update() {
            lr.SetPosition(0, relativeTransform.position);
            lr.SetPosition(1, transform.position);
        }

        public Vector3 ProjectPointToSphere(Vector3 point, Vector3 center, float radius) {
            return center + radius * (point - center).normalized;
        }

        public Vector3 ProjectPointToCircle(Vector3 point, Vector3 center, Vector3 circleNormal, float radius) {
            //sphereWhite.localPosition = Vector3.ProjectOnPlane(point, circleNormal);
            return ProjectPointToSphere(point, center, radius);
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData) {
            relativeTransform = transform.parent.GetChild(1).GetChild(0);
            Vector3 pointerPos = eventData.Pointer.Position;
            Vector3 center = relativeTransform.position;
            Vector3 norm = relativeTransform.right; // not needed anymore
            Vector3 forwardReference = relativeTransform.forward;
            pointerPos.x = relativeTransform.position.x;
            Debug.DrawRay(relativeTransform.position, forwardReference);


            Vector3 potentialPosition = ProjectPointToCircle(pointerPos, center, norm, RADIUS);
            float angleFromOutward = Vector3.SignedAngle(forwardReference, potentialPosition, norm);
            Debug.Log(angleFromOutward);
            // -180 to -160 is up
            // 180 to 160 is down
            float percent = 0;

            if (angleFromOutward < 0) {
                if (Mathf.Abs(angleFromOutward) < 180 - MAX_ANGLE) {
                    potentialPosition = center + (Quaternion.Euler(-180 + MAX_ANGLE, 0, 0) * forwardReference - center).normalized * RADIUS;
                    angleFromOutward = -180 + MAX_ANGLE;
                }
                percent = (angleFromOutward + 180) / MAX_ANGLE;
            }
            else {
                if (Mathf.Abs(angleFromOutward) < 180 - MAX_ANGLE) {
                    potentialPosition = center + (Quaternion.Euler(180 - MAX_ANGLE, 0, 0) * forwardReference - center).normalized * RADIUS;
                    angleFromOutward = 180 - MAX_ANGLE;
                }
                percent = (angleFromOutward - 180) / MAX_ANGLE;
            }


            transform.position = potentialPosition;
            if (FindObjectOfType<KuriManager>()) {
                KuriManager.instance.SetVelocity(percent);
            }
            textMesh.SetText(percent.ToString("#.00"));
        }

        private bool IsNearPointer(IMixedRealityPointer pointer) {
            return (pointer is IMixedRealityNearPointer);
        }

        // Needed for IMixedRealityPointerHandler
        public void OnPointerUp(MixedRealityPointerEventData eventData) {
        }
        public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        }
        public void OnPointerDown(MixedRealityPointerEventData eventData) {
            Vector3 initialGrabPoint = Quaternion.Inverse(eventData.Pointer.Rotation) * (eventData.Pointer.Result.Details.Point - eventData.Pointer.Position);
            curPointer = new PointerData(eventData.Pointer, initialGrabPoint);
            if (curPointer.IsNearPointer()) {
                Debug.Log("near");
            }
        }
    }
}
