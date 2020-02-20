using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollidedFaces: MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.Log("Collided");
            Debug.Log("Collide Point:" + contact.point);
            Debug.Log("Collide Normal:" + contact.normal);
            Debug.Log("Raycast from:" + transform.position);
            Debug.Log("Raycast to:" + (contact.point - transform.position));

            RaycastHit hit;
            if (!Physics.Raycast(transform.position, Vector3.Normalize(contact.point - transform.position), out hit))
            {
                Debug.Log("Nothing was hit");
                return;
            }
            Debug.Log(hit.transform.gameObject.name);

            MeshCollider meshCollider = hit.collider as MeshCollider;
            if (meshCollider == null || meshCollider.sharedMesh == null)
            {
                Debug.Log("No Mesh Collider was hit");
                return;
            }
            Debug.Log("" + hit.point + hit.normal + hit.triangleIndex);
            DeleteFace(collision.transform.gameObject, hit.triangleIndex);
        }
        
    }

    void DeleteFace(GameObject other, int index)
    {
        if (index < 0)
            return;
        Destroy(other.GetComponent<MeshCollider>());
        Mesh mesh = other.GetComponent<MeshFilter>().mesh;

        List<int> oldTriangles = new List<int>(mesh.triangles);
        oldTriangles.RemoveAt(index * 3);
        oldTriangles.RemoveAt(index * 3);
        oldTriangles.RemoveAt(index * 3);
        int[] newTriangles = oldTriangles.ToArray();

        other.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        other.AddComponent<MeshCollider>();
    }
}
