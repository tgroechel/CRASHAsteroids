using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOfOtherBlock : MonoBehaviour {

    public void ChangeMyColor() {
        gameObject.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
    }


}
