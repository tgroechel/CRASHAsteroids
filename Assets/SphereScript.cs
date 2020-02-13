using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour
{
    
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 0, 4);
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
    }

    public void Lauch()
    {
        rb.velocity = new Vector3(Camera.main.transform.forward.x * 1,
            Camera.main.transform.forward.y * 15,
            Camera.main.transform.forward.z * 15);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Vector3 vel = Vector3.Normalize(rb.velocity) * 15;
        rb.velocity = vel;
    }
}
