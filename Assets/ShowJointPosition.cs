using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowJointPosition : MonoBehaviour
{
    public static TextMeshProUGUI textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.SetText("Joint Position relative to wrist will be shown here.");
    }
}
