using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using eDmitriyAssets.NavmeshLinksGenerator;

public class BakeNavMeshRuntime : MonoBehaviour
{
    private NavMeshSurface surface;
    private NavMeshLinks_AutoPlacer autoplacer;
    private NavMeshLinks_AutoPlacer_Editor editor;

    // Start is called before the first frame update
    void Start()
    {
        // Get the NavMeshSurface component
        surface = GetComponent<NavMeshSurface>();

        // Get the NavMeshLinks_AutoPlacer component
        autoplacer = GetComponent<NavMeshLinks_AutoPlacer>();

        // Get the NavMeshLinks_AutoPlacer_Editor component
        editor = GetComponent<NavMeshLinks_AutoPlacer_Editor>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            // Build NavMesh
            surface.BuildNavMesh();
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            // Generate NavMesh Links
            GetComponent<NavMeshLinks_AutoPlacer>().Generate();
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            // Remove NavMesh
            surface.RemoveData();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            // Remove NavMesh Links
            GetComponent<NavMeshLinks_AutoPlacer>().ClearLinks();
        }
    }
}
