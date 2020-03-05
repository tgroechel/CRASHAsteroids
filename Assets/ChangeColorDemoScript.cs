using Microsoft.MixedReality.Toolkit.UI;
using System;
using UnityEngine;

namespace Tomato {
    public class ChangeColorDemoScript : MonoBehaviour {
        ManipulationHandler manipHandler;
        MeshRenderer meshrend;
        private void Start() {
            meshrend = GetComponent<MeshRenderer>();
            manipHandler = GetComponent<ManipulationHandler>();
            manipHandler.OnHoverEntered.AddListener(Grapes);
            manipHandler.OnManipulationStarted.AddListener(ChangeColorOnClick);
        }

        private void Grapes(ManipulationEventData arg0) {
            Debug.Log("You are hovering me, grapes");
        }

        private void ChangeColorOnClick(ManipulationEventData arg0) {
            meshrend.material.color = UnityEngine.Random.ColorHSV();
        }

        public void ToggleMyActiveState() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

    }
}
