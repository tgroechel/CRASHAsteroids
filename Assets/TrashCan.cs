using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomato {
    public class TrashCan : MonoBehaviour {

        public void OnTriggerEnter(Collider other) {
            other.GetComponent<ChangeColorDemoScript>().ToggleMyActiveState();
        }

    }
}
