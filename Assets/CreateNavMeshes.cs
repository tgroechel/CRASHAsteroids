using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNavMeshes : MonoBehaviour
{
    private static bool meshesAttached;
    private static GameObject navMeshPrefab; 

    // Start is called before the first frame update
    void Start()
    {
        meshesAttached = false;
        navMeshPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.navMesh) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !meshesAttached)
        {
            attachNavMeshes();
            meshesAttached = true;
        }
    }

    // Attach NavMesh prefab to each wall
    void attachNavMeshes()
    {
        // Angle by which the navMesh prefab should be rotated locally
        float angleX = -90;
        float angleY = 0;
        float angleZ = 0;

        int no_of_children = transform.childCount;
        for (int i = 0; i < no_of_children; i++)    // for each wall
        {
            // Current wall
            GameObject currentWall = transform.GetChild(i).gameObject;

            // Get first child of the wall
            GameObject child = currentWall.transform.GetChild(0).gameObject;

            // Instantiate new NavMesh object from prefab and make the child the parent of navMesh
            GameObject navMesh = Instantiate(navMeshPrefab, child.transform);

            // Make the navMesh's location and rotation the same as that of the parent
            navMesh.transform.localPosition = Vector3.zero;
            navMesh.transform.localRotation = Quaternion.identity;

            // Rotating the navMesh so that its y-axis protrudes out of the wall (its parent)
            navMesh.transform.Rotate(angleX, angleY, angleZ, Space.Self);
        }
    }
}
