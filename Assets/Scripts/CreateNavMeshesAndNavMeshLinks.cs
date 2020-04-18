using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class CreateNavMeshesAndNavMeshLinks : MonoBehaviour
{
    public NavMeshSurface floorNavMesh;
    public GameObject navMeshesObject, bossObject, kuri;
    public WaterTightDetector waterTightDetector;
    private static GameObject navMeshPrefab;
    private int agentTypeID;
    private bool navMeshBuildDone;

    void Awake()
    {
        // Loading NavMesh prefab
        navMeshPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.navMesh) as GameObject;

        // Obtaining agent ID
        agentTypeID = floorNavMesh.agentTypeID;

        // Initialising navMeshBuildDone to false
        navMeshBuildDone = false;
    }

    private void Update()
    {
        // If Scene Understanding is complete, build navmeshes and navmeshlinks
        // Do this only once
        if(waterTightDetector.isWaterTight && !navMeshBuildDone)
        {
            // Set navMeshBuildDone to true
            navMeshBuildDone = true;

            // Do all the heavy work in a coroutine to prevent the game from freezing
            StartCoroutine(BuildNavMeshesAndNavMeshLinks());
        }

    }

    // Attach NavMesh prefab to each wall
    private IEnumerator AttachNavMeshes()
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

            yield return null;
        }
    }

    private IEnumerator CreateWallLinks() // Links between walls and floor
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

            yield return null;
        }
    }

    private IEnumerator BuildNavMeshesAndNavMeshLinks()
    {
        // Attach NavMeshes to every child surface
        yield return StartCoroutine(AttachNavMeshes());

        // Note: NavMeshes (NavMesh prefab) get automatically built after attaching them to an object due to BakeNavMeshRuntime script

        // Create NavMeshLinks for every child NavMesh
        // Note: This has to take place after NavMeshes have been attached
        yield return StartCoroutine(CreateWallLinks());

        // Activating objects
        // Note: They must be activated in this exact same order to prevent warnings!

        // Activate NavMeshes object
        navMeshesObject.SetActive(true);

        // Activate Kuri object
        kuri.SetActive(true);

        // Activate Boss object
        bossObject.SetActive(true);
    }
}
