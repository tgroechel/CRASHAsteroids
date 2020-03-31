using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshLinks : MonoBehaviour
{
    private static bool linksCreated;
    public int agentTypeID;
    public NavMeshSurface floorNavMesh;

    // Start is called before the first frame update
    void Start()
    {
        linksCreated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L) && !linksCreated)
        {
            agentTypeID = floorNavMesh.agentTypeID;
            createWallLinks();
            linksCreated = true;
        }
    }

    void createWallLinks() // Links between walls and floor
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
