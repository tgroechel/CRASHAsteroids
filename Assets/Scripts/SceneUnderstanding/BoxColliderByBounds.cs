using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderByBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        gameObject.AddComponent<BoxCollider>();
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.size = mesh.bounds.size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
