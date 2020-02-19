using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

    private Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();

    }

    public void ResetPosition() {
        transform.position = new Vector3(0, 0, 4);
        rb.velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            ResetPosition();
        }
    }

    public void Lauch() {
        rb.velocity = Camera.main.transform.forward * speed;
    }

    public void OnCollisionEnter(Collision collision) {
        rb.velocity = Vector3.Normalize(rb.velocity) * speed;
    }
}
