using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class PlayerEffectPlane : MonoBehaviour {
        MeshRenderer meshrend;
        Color origColor;
        private void Awake() {
            meshrend = GetComponent<MeshRenderer>();
            origColor = meshrend.material.color;
        }

        public void SetTransparency(float percent) {
            Color n = origColor;
            n.a = percent;
            meshrend.material.color = n;
        }
    }
}
