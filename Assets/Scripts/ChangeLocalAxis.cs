using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLocalAxis : MonoBehaviour
{
    public Vector3 originalUp, originalForward, originalRight;

    // Start is called before the first frame update
    void Start()
    {
        originalUp = transform.up;              // Y
        originalForward = transform.forward;    // Z
        originalRight = transform.right;        // X
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.up = originalForward;       // Y = Z            
        }
    }
}
