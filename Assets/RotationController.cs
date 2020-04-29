using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crash {
    public class RotationController : MonoBehaviour, IMixedRealityPointerHandler {
        public const float RADIUS = 0.8f;
        LineRenderer lr;
        TextMeshProUGUI textMesh;
        public Transform relativeTransform;
        PointerData curPointer;
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
            return ProjectPointToSphere(point, center, radius);
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData) {
            Vector3 pointerPos = eventData.Pointer.Position;
            Vector3 center = relativeTransform.position;
            Vector3 norm = -transform.parent.up;
            pointerPos.y = relativeTransform.position.y;

            Vector3 potentialPosition = ProjectPointToCircle(pointerPos, center, norm, RADIUS);
            float angleFromOutward = Vector3.SignedAngle(transform.parent.forward, potentialPosition, norm);
            Debug.Log(angleFromOutward);
            transform.localPosition = potentialPosition;
            KuriManager.instance.SetRotation(angleFromOutward);
            textMesh.SetText(angleFromOutward.ToString("#.00"));
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
