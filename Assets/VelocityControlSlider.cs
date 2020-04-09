using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Crash {
    public class VelocityControlSlider : MonoBehaviour {
        PinchSlider pinchSlider;
        private void Awake() {
            pinchSlider = GetComponent<PinchSlider>();
            pinchSlider.OnValueUpdated.AddListener(UpdateRotationValue);
        }

        private void UpdateRotationValue(SliderEventData sliderEvent) {
            float newRotation = (sliderEvent.NewValue - 0.5f) * 357;
            KuriManager.instance.SetRotation(newRotation);
        }
    }
}