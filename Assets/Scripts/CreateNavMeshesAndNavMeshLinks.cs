using Microsoft.MixedReality.SceneUnderstanding.Samples.Unity;
using Sid;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
public class CreateNavMeshesAndNavMeshLinks : MonoBehaviour {
    public NavMeshSurface floorNavMesh;
    public GameObject navMeshesObject, enemiesObject;
    public WaterTightDetector waterTightDetector;
    public bool enableWallFloorLinks, enableWallCeilingLinks, showLoading;
    public float percentageCompleted, delayBeforeActivation, addedHeightBoss, linkOffset;
    public int bossTriangleIndex, kuriTriangleIndex;
    public AudioSource globalAudioSource;
    public AudioClip gameLoadSound;
    public float volume;
    public GameObject spawnEffect;
    public AudioClip spawnSound;
    public AudioSource spawnSource;
    private float doneCount;
    private bool pastEnableWallFloorLinks, pastEnableWallCeilingLinks;
    private static GameObject navMeshPrefab, bossPrefab, kuriPrefab;
    private int agentTypeID;
    private bool navMeshBuildDone, updateLinks, updateLinksRunning;
    private List<NavMeshLink> wallFloorLinks, wallCeilingLinks;
    private List<BakeNavMeshRuntime> floorNavMeshBakeScripts;
    private CapsuleCollider kuriCollider;

    void Awake() {
        // Loading NavMesh prefab
        navMeshPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.navMesh) as GameObject;

        // Loading Boss prefab
        bossPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.boss) as GameObject;

        // Loading Kuri prefab
        kuriPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.kuri) as GameObject;

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

        // Creating lost to store floor navmesh objects' BakeNavMeshRuntime components
        floorNavMeshBakeScripts = new List<BakeNavMeshRuntime>();
        for (int i = 0; i < navMeshesObject.transform.childCount; i++) {
            floorNavMeshBakeScripts.Add(navMeshesObject.transform.GetChild(i).GetComponent<BakeNavMeshRuntime>());
        }

        // Obtaining CapsuleCollider on Kuri
        kuriCollider = kuriPrefab.GetComponent<CapsuleCollider>();

        // Start game loading sound
        PlayGameLoadSound();
    }

    private void Update() {
        // If Scene Understanding is complete, build navmeshes and navmeshlinks
        // Do this only once for the entire duration of the game
        if (waterTightDetector.isWaterTight && !navMeshBuildDone) {
            // Set navMeshBuildDone to true
            navMeshBuildDone = true;

            // Do all the heavy work in a coroutine to prevent the game from freezing
            StartCoroutine(BuildNavMeshesAndNavMeshLinks());
        }

        // Update State of NavMeshLinks according to the checkboxes chosen
        // Note: Update will happen only when past bool is not equal to current bool
        if (updateLinks) {
            if (pastEnableWallFloorLinks != enableWallFloorLinks && !updateLinksRunning) {
                updateLinksRunning = true;
                pastEnableWallFloorLinks = enableWallFloorLinks;
                changeStateOfLinks(wallFloorLinks, enableWallFloorLinks);
            }

            if (pastEnableWallCeilingLinks != enableWallCeilingLinks && !updateLinksRunning) {
                updateLinksRunning = true;
                pastEnableWallCeilingLinks = enableWallCeilingLinks;
                changeStateOfLinks(wallCeilingLinks, enableWallCeilingLinks);
            }
        }

    }

    // Attach NavMesh prefab to each wall
    private IEnumerator AttachNavMeshes() {
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
            float startY = -collider.size.y / 2 + linkOffset;
            float startZ = 0;
            sc.startPoint = new Vector3(startX, startY, startZ);

            // Set end point of link
            float endX = 0;
            float endY = -collider.size.y / 2;
            float endZ = -linkOffset;
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
            startY = collider.size.y / 2 - linkOffset;
            startZ = 0;
            sc.startPoint = new Vector3(startX, startY, startZ);

            // Set end point of link
            endX = 0;
            endY = collider.size.y / 2;
            endZ = -linkOffset;
            sc.endPoint = new Vector3(endX, endY, endZ);

            // Set width of link
            sc.width = collider.size.x;

            // Increase done count and re-evaulate percentageCompleted
            doneCount++;
            percentageCompleted = (doneCount / (2 * no_of_children)) * 100;

            yield return null;
        }
    }

    private IEnumerator BuildNavMeshesAndNavMeshLinks() {
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

        // While all builds are complete
        int count = 0;
        while (count != floorNavMeshBakeScripts.Count) {
            count = 0;
            foreach (BakeNavMeshRuntime floorNavMeshBakeScript in floorNavMeshBakeScripts) {
                if (!floorNavMeshBakeScript.surface.IsInvoking("BuildNavMesh"))
                    count++;
            }
            yield return null;
        }

        FindObjectOfType<SceneUnderstandingDataProvider>().DisableContinualRetrieval(true);

        // Wait for a few seconds before instantiating Kuri and Boss objects
        yield return new WaitForSeconds(delayBeforeActivation);

        // Reset showLoading to false
        showLoading = false;

        // Get spawnPosition for the boss
        Vector3 spawnPosition = GetAgentSpawnPosition("BossWalkable", bossTriangleIndex);

        // Show particle effects at spawn position and play spawn sound
        spawnEffect.transform.position = spawnPosition;
        spawnEffect.SetActive(true);
        spawnSource.Stop();
        spawnSource.loop = true;
        spawnSource.clip = spawnSound;
        spawnSource.volume = volume;
        spawnSource.Play();

        yield return new WaitForSeconds(5.0f);

        // Instantiate Boss object
        GameObject bossObject = Instantiate(bossPrefab, enemiesObject.transform);
        NavMeshAgent bossAgent = bossObject.GetComponent<NavMeshAgent>();
        bossAgent.Warp(spawnPosition);
        bossAgent.enabled = false;
        bossAgent.enabled = true;
        bossObject.name = ResourcePathManager.boss;
        print("Boss Location is -> " + bossObject.transform.position);

        // Stop spawn particle effects
        spawnSource.Stop();
        spawnSource.loop = false;
        spawnEffect.SetActive(false);

        // Instantiate Kuri object and initialize some of her attributes
        spawnPosition = GetAgentSpawnPosition("KuriWalkable", kuriTriangleIndex);
        spawnPosition.y += kuriCollider.height + 0.2f;
        GameObject kuriObject = Instantiate(kuriPrefab, enemiesObject.transform.parent.transform);
        NavMeshAgent kuriAgent = kuriObject.GetComponent<NavMeshAgent>();
        kuriAgent.Warp(spawnPosition);
        kuriAgent.enabled = false;
        kuriAgent.enabled = true;
        kuriObject.name = ResourcePathManager.kuri;
        kuriObject.GetComponent<ComputePathToBoss>().boss = bossObject;
        print("Kuri Location is -> " + kuriObject.transform.position);

        // Set Kuri for Boss
        bossObject.GetComponent<HealthManager>().kuri = kuriObject;

        // Stop Game Loading Sound first
        globalAudioSource.Stop();

        /*// Bake floor meshes again
        foreach (BakeNavMeshRuntime floorNavMeshBakeScript in floorNavMeshBakeScripts)
            floorNavMeshBakeScript.surface.BuildNavMesh();*/

        // Set updateLinks to true to allow NavMeshLinks to be updated.
        // We want links to be updated to user settings
        // only after NavMeshes and NavMeshLinks have been built.
        updateLinks = true;
    }

    void changeStateOfLinks(List<NavMeshLink> navMeshLinks, bool setState) {
        // Change state of each link
        navMeshLinks.ForEach(link => {
            if (link.enabled != setState)
                link.enabled = setState;
        });

        // Change updateLinksRunning to false
        updateLinksRunning = false;
    }

    // Get Random Point on a Navmesh surface (For future reference)
    public static bool GetRandomPoint(Vector3 center, float maxDistance, int areaMask, out Vector3 position) {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        bool gotPoint = NavMesh.SamplePosition(randomPos, out hit, maxDistance, areaMask);
        position = hit.position;

        return gotPoint;
    }

    // Get the first point on the NavMesh having area type as areaName
    private Vector3 GetAgentSpawnPosition(string areaName, int index) {
        // Obtain all the info about the triangles in the Global Common NavMesh
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        // Find the first point on the area named areaName
        int areaIndex = NavMesh.GetAreaFromName(areaName);
        int count = 0;
        for (int i = 0; i < triangulation.indices.Length; i += 3) {
            var triangleIndex = i / 3;
            var i1 = triangulation.indices[i];
            Vector3 p1 = triangulation.vertices[i1];
            int currAreaIndex = triangulation.areas[triangleIndex];
            if (currAreaIndex == areaIndex) {
                count++;
                if (count == index)
                    return p1;
            }

        }

        // This statement is never reached
        // But without it, VisualStudio gives a compile time error
        return Vector3.zero;
    }

    // As a future reference if we want to show navMeshes in the "Game" itself
    void showNavMeshes() {
        // Obtain all the info about the triangles in the Global Common NavMesh
        NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

        Material material = null;

        if (material == null) {
            return;
        }
        GL.PushMatrix();

        material.SetPass(0);
        GL.Begin(GL.TRIANGLES);

        for (int i = 0; i < triangulation.indices.Length; i += 3) {
            var triangleIndex = i / 3;
            var areaIndex = triangulation.areas[triangleIndex];

            var i1 = triangulation.indices[i];
            var i2 = triangulation.indices[i + 1];
            var i3 = triangulation.indices[i + 2];
            var p1 = triangulation.vertices[i1];
            var p2 = triangulation.vertices[i2];
            var p3 = triangulation.vertices[i3];

            Color color;
            Color walkableColor = Color.green;
            Color nonWalkableColor = Color.blue;
            Color unknownColor = Color.yellow;
            switch (areaIndex) {
                case 0:
                    color = walkableColor; break;
                case 1:
                    color = nonWalkableColor; break;
                default:
                    color = unknownColor; break;
            }
            GL.Color(color);
            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.Vertex(p3);

        }
        GL.End();

        GL.PopMatrix();
    }

    public void PlayGameLoadSound() {
        globalAudioSource.Stop();
        globalAudioSource.loop = true;
        globalAudioSource.clip = gameLoadSound;
        globalAudioSource.volume = volume;
        globalAudioSource.Play();
    }
}
