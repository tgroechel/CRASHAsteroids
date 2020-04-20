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
    public bool enableWallFloorLinks, enableWallCeilingLinks, showLoading;
    public float percentageCompleted;
    private float doneCount;
    private bool pastEnableWallFloorLinks, pastEnableWallCeilingLinks;
    private static GameObject navMeshPrefab;
    private int agentTypeID;
    private bool navMeshBuildDone, updateLinks, updateLinksRunning;
    private List<NavMeshLink> wallFloorLinks, wallCeilingLinks;


    void Awake()
    {
        // Loading NavMesh prefab
        navMeshPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.navMesh) as GameObject;

        // Obtaining agent ID
        agentTypeID = floorNavMesh.agentTypeID;

        // Initialising navMeshBuildDone to false
        navMeshBuildDone = false;

        // Initialising the state of the two types of links
        pastEnableWallFloorLinks = false;
        pastEnableWallCeilingLinks = false;

        // Initialising updateLinks to false (both types of links are present by default)
        updateLinks = false;

        // This is to make sure that only one updateLinks call is running at a time
        // to prevent erroneous behaviour
        updateLinksRunning = false; 

        // Creating lists to store wall-floor and wall-ceiling links
        wallFloorLinks = new List<NavMeshLink>();
        wallCeilingLinks = new List<NavMeshLink>();

        // Initialising showLoading to false and doneCount to 0
        showLoading = false;
        doneCount = 0;
    }

    private void Update()
    {
        // If Scene Understanding is complete, build navmeshes and navmeshlinks
        // Do this only once for the entire duration of the game
        if(waterTightDetector.isWaterTight && !navMeshBuildDone)
        {
            // Set navMeshBuildDone to true
            navMeshBuildDone = true;

            // Do all the heavy work in a coroutine to prevent the game from freezing
            StartCoroutine(BuildNavMeshesAndNavMeshLinks());
        }

        // Update State of NavMeshLinks according to the checkboxes chosen
        // Note: Update will happen only when past bool is not equal to current bool
        if(updateLinks)
        {
            if(pastEnableWallFloorLinks != enableWallFloorLinks && !updateLinksRunning)
            {
                updateLinksRunning = true;
                pastEnableWallFloorLinks = enableWallFloorLinks;
                changeStateOfLinks(wallFloorLinks, enableWallFloorLinks);
            }
            
            if(pastEnableWallCeilingLinks != enableWallCeilingLinks && !updateLinksRunning)
            {
                updateLinksRunning = true;
                pastEnableWallCeilingLinks = enableWallCeilingLinks;
                changeStateOfLinks(wallCeilingLinks, enableWallCeilingLinks);
            }
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

            // Increase done count and re-evaulate percentageCompleted
            doneCount++;
            percentageCompleted = (doneCount / (2 * no_of_children)) * 100;

            yield return null;
        }
    }

    private IEnumerator CreateWallLinks() // Links between walls-floor and wall-ceiling
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

            /* Two kinds of NavMeshLinks exist:
             * 1. Links between walls and the floor
             * 2. Links between walls and the ceiling */

            // 1. Add a NavMeshLink component (Link between wall and floor)
            NavMeshLink sc = child.AddComponent<NavMeshLink>() as NavMeshLink;
            wallFloorLinks.Add(sc);

            // Set agentType for the link
            sc.agentTypeID = agentTypeID;

            // Before setting the start and end points,
            // Set the state of the link
            sc.enabled = pastEnableWallFloorLinks;

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

            // 2. Add a NavMeshLink component (Link between wall and ceiling)
            sc = child.AddComponent<NavMeshLink>() as NavMeshLink;
            wallCeilingLinks.Add(sc);

            // Set agentType for the link
            sc.agentTypeID = agentTypeID;

            // Before setting the start and end points,
            // Set the state of the link
            sc.enabled = pastEnableWallCeilingLinks;

            // Set start point of link
            startX = 0;
            startY = collider.size.y / 2 - offset;
            startZ = 0;
            sc.startPoint = new Vector3(startX, startY, startZ);

            // Set end point of link
            endX = 0;
            endY = collider.size.y / 2;
            endZ = -offset;
            sc.endPoint = new Vector3(endX, endY, endZ);

            // Set width of link
            sc.width = collider.size.x;

            // Increase done count and re-evaulate percentageCompleted
            doneCount++;
            percentageCompleted = (doneCount / (2 * no_of_children)) * 100;

            yield return null;
        }
    }

    private IEnumerator BuildNavMeshesAndNavMeshLinks()
    {
        // Set showLoading to true
        showLoading = true;

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

        // Reset showLoading to false
        showLoading = false;

        // Set updateLinks to true to allow NavMeshLinks to be updated.
        // We want links to be updated to user settings
        // only after NavMeshes and NavMeshLinks have been built.
        updateLinks = true;
    }

    void changeStateOfLinks(List<NavMeshLink> navMeshLinks, bool setState)
    {
        // Change state of each link
        navMeshLinks.ForEach(link => {
            if (link.enabled != setState)
                link.enabled = setState;
        });

        // Change updateLinksRunning to false
        updateLinksRunning = false;
    }
}
