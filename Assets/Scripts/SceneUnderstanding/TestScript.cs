using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject root = GameObject.Find("SceneUnderstanding");
        if (root != null)
        {
            Debug.Log("SceneUnderstanding Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
