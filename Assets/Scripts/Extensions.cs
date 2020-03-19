using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public static class Extensions {

        // transform extension
        public static void SnapToParent(this Transform t, Transform prospectiveParent) {
            t.rotation = prospectiveParent.rotation;
            t.SetParent(prospectiveParent);
        }

        public static void SnapToParent(this Transform t, Transform prospectiveParent, Vector3 newLocalPosition) {
            t.rotation = prospectiveParent.rotation;
            t.SetParent(prospectiveParent);
            t.localPosition = newLocalPosition;
        }

        public static void RotateTowardsUser(this Transform t) {
            t.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        public static void SnapToSnapManager(this Transform t) {
            SnapToParent(t, SnapColliderManager.instance.transform);
            t.localScale = Vector3.one;
        }
    }
}
