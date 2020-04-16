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
    void Awake()
    {
        // Get the NavMeshSurface component
        surface = GetComponent<NavMeshSurface>();

        // Build NavMesh
        surface.BuildNavMesh();

        /* Left the following statements commented as a reference for future work
        // Remove NavMesh
        surface.RemoveData();

        // Get the NavMeshLinks_AutoPlacer component
        autoplacer = GetComponent<NavMeshLinks_AutoPlacer>();

        // Generate NavMesh Links
        autoplacer.Generate();

        // Remove NavMesh Links
        autoplacer.ClearLinks();

        // Get the NavMeshLinks_AutoPlacer_Editor component
        editor = GetComponent<NavMeshLinks_AutoPlacer_Editor>();
        */
    }
}
