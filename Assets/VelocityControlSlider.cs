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
            KuriManager.instance.SetRotation((sliderEvent.NewValue - 0.5f) * 359);
        }
    }
}