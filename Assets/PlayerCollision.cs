using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Vector3 vel = Vector3.Normalize(collision.rigidbody.velocity) * 15;
        //collision.rigidbody.velocity = vel;
    }
}
