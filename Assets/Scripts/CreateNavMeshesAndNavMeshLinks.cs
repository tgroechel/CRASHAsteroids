using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshesAndNavMeshLinks : MonoBehaviour
{
    public NavMeshSurface floorNavMesh;
    private static GameObject navMeshPrefab;
    private int agentTypeID;

    void Awake()
    {
        // Attach NavMeshes to every child surface
        navMeshPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.navMesh) as GameObject;
        attachNavMeshes();

        // Create NavMeshLinks for every child NavMesh
        // Note: This has to take place after NavMeshes have been attached
        agentTypeID = floorNavMesh.agentTypeID;
        createWallLinks();
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

            // Add NavMesh only if child has name 'Wall'
            //if(child.name == "Wall")
            //{
                // Instantiate new NavMesh object from prefab and make the child the parent of navMesh
                GameObject navMesh = Instantiate(navMeshPrefab, child.transform);

                // Make the navMesh's location and rotation the same as that of the parent
                navMesh.transform.localPosition = Vector3.zero;
                navMesh.transform.localRotation = Quaternion.identity;

                // Rotating the navMesh so that its y-axis protrudes out of the wall (its parent)
                navMesh.transform.Rotate(angleX, angleY, angleZ, Space.Self);
            //}            
        }
    }

    public void createWallLinks() // Links between walls and floor
    {
        float offset = 0.2f;
        int no_of_children = transform.childCount;
        for (int i = 0; i < no_of_children; i++)    // for each wall
        {
            // Current wall
            GameObject currentWall = transform.GetChild(i).gameObject;

            // Get first child of the wall
            GameObject child = currentWall.transform.GetChild(0).gameObject;

            // Get collider attached to the current child
            BoxCollider collider = child.GetComponent<BoxCollider>();

            // Add a NavMeshLink component
            NavMeshLink sc = child.AddComponent<NavMeshLink>() as NavMeshLink;

            // Set agentType for the link
            sc.agentTypeID = agentTypeID;

            // Set start point of link
            float startX = 0;
            float startY = -collider.size.y / 2 + offset;
            float startZ = 0;
            sc.startPoint = new Vector3(startX, startY, startZ);

            // Set end point of link
            float endX = 0;
            float endY = -collider.size.y / 2;
            float endZ = -offset;
            sc.endPoint = new Vector3(endX, endY, endZ);

            // Set width of link
            sc.width = collider.size.x;
        }
    }
}
