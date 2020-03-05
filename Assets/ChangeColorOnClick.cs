using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Junk {
    public class ChangeColorOnClick : MonoBehaviour {
        ManipulationHandler manhan;
        MeshRenderer meshrend;

        private void Awake() {
            manhan = GetComponent<ManipulationHandler>();
            meshrend = GetComponent<MeshRenderer>();
            manhan.OnHoverEntered.AddListener(ChangeColor);
            FindObjectOfType<PressableButtonHoloLens2>().GetComponent<PressableButtonHoloLens2>().TouchBegin.AddListener(ToggleEnable);
        }

        private void ChangeColor(ManipulationEventData arg0) {
            meshrend.material.color = UnityEngine.Random.ColorHSV();
        }

        public void ToggleEnable() {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
