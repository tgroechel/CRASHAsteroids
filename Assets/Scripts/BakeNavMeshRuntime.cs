﻿using eDmitriyAssets.NavmeshLinksGenerator;
using UnityEngine;
using UnityEngine.AI;

public class BakeNavMeshRuntime : MonoBehaviour {
    public NavMeshSurface surface;
    private NavMeshLinks_AutoPlacer autoplacer;
#if UNITY_EDITOR
    private NavMeshLinks_AutoPlacer_Editor editor;
#endif

    private void Awake()
    {
        // Get the NavMeshSurface component
        surface = GetComponent<NavMeshSurface>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
