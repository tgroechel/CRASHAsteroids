using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Crash {
    public class VelocityControlSlider : MonoBehaviour {
        PinchSlider pinchSlider;
        private void Awake() {
            pinchSlider = GetComponent<PinchSlider>();
            pinchSlider.OnValueUpdated.AddListener(UpdateVelocityValue);
        }

        private void UpdateVelocityValue(SliderEventData sliderEvent) {
            // Slider is value 0 to 1
            KuriManager.instance.SetVelocity((sliderEvent.NewValue - 0.5f));
        }
    }
}