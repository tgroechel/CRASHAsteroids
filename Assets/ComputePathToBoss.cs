using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sid;

public class ComputePathToBoss : MonoBehaviour
{
    public GameObject boss;
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;
    private bool pathDrawn;
    private NavMeshPath path;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    private GameObject locationEffectsKuri;
    private bool kuriDestinationCalculated;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        animator = boss.GetComponent<Animator>();

        // Initialize states of variables
        agent.updateRotation = false;
        pathDrawn = true;

        // Get Collider Component
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Get location effects for Kuri
        locationEffectsKuri = GameObject.Find("LocationEffects").transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending)
        {
            // Draw path if not already drawn
            if (!pathDrawn)
            {
                DrawPath(path);
                pathDrawn = true;
            }

            // Calculate and show path only if the boss is deactivated
            // and is visible in the scene (Enemies object is active and Boss is present and active)
            if ((boss != null ? boss.activeSelf : false) && !animator.GetBool("Activate"))
            {
                // Wait till kuriDestination is calculated
                kuriDestinationCalculated = false;
                StartCoroutine(WaitForKuriDestination());

                if(kuriDestinationCalculated)
                {
                    // Calculate path to boss
                    agent.CalculatePath(EnemySoundsScript.kuriDestination, path = new NavMeshPath());

                    // Set pathDrawn to false
                    pathDrawn = false;
                }
            }
            else 
            {
                // Remove drawn path
                lineRenderer.positionCount = 0;
            }
        }
    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        lineRenderer.positionCount = path.corners.Length; //set the array of positions to the amount of corners

        Vector3 startPoint = agent.transform.position;
        startPoint.y -= capsuleCollider.height - 0.1f;
        lineRenderer.SetPosition(0, startPoint); //set the first point to the current position of the GameObject

        for (var i = 1; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }

    public IEnumerator WaitForKuriDestination()
    {
        while (!EnemySoundsScript.kuriDestinationCalculated)
        {
            yield return null;
        }
        kuriDestinationCalculated = true;
    }
}
