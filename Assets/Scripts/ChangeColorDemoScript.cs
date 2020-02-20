using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace Sid
{
    public class ChangeColorDemoScript : MonoBehaviour
    {
        MeshRenderer meshrend;
        ManipulationHandler manipHandler;

        private void Start()
        {
            meshrend = GetComponent<MeshRenderer>();
            manipHandler = GetComponent<ManipulationHandler>();
            manipHandler.OnManipulationStarted.AddListener(ChangeColorOnClick);
        }

        private void ChangeColorOnClick(ManipulationEventData arg0)
        {
            meshrend.material.color = UnityEngine.Random.ColorHSV();
        }

        public void ToggleMyActiveState()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }   
}
