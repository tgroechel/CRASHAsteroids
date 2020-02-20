using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyClickedFaces: MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                DeleteFace(hit.triangleIndex);
            }
        }
    }

    void DeleteFace(int index)
    {
        if (index < 0)
            return;
        Destroy(this.gameObject.GetComponent<MeshCollider>());
        Mesh mesh = transform.GetComponent<MeshFilter>().mesh;

        List<int> oldTriangles = new List<int>(mesh.triangles);
        oldTriangles.RemoveAt(index * 3);
        oldTriangles.RemoveAt(index * 3);
        oldTriangles.RemoveAt(index * 3);
        int[] newTriangles = oldTriangles.ToArray();

        transform.GetComponent<MeshFilter>().mesh.triangles = newTriangles;
        gameObject.AddComponent<MeshCollider>();
    }

    private void PrintMeshData(Mesh mesh)
    {
        Debug.Log("mesh.vertices.Length:" + mesh.vertices.Length);
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            Debug.Log(mesh.vertices[i]);
        }

        Debug.Log("mesh.triangles.Length:" + mesh.triangles.Length);
        for (int i=0; i<mesh.triangles.Length/3; i++)
        {
            Debug.Log(mesh.triangles[i * 3 + 0] + ", " + 
                      mesh.triangles[i * 3 + 1] + "," + 
                      mesh.triangles[i * 3 + 2]);
        }
        
    }
}
