using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProjectile : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");
            ResetPosition();
            Launch();
        }


        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");


        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed middle click.");
            ResetPosition();
        }
        
    }

    public void Launch()
    {
        rb.AddForce(Camera.main.transform.forward*10, ForceMode.Impulse);
    }

    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = Camera.main.transform.position + Camera.main.transform.forward;
    }
}
