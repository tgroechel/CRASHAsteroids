using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class ThrottleController : MonoBehaviour, IMixedRealityPointerHandler {
        public void OnPointerClicked(MixedRealityPointerEventData eventData) {

        }

        public void OnPointerDown(MixedRealityPointerEventData eventData) {

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
            Vector3 norm = transform.right;

            transform.localPosition = ProjectPointToCircle(pointerPos, center, norm, 1.2f);

        }

        public void OnPointerUp(MixedRealityPointerEventData eventData) {

        }
    }
}
