using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MinionFollowPlayerScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public ThirdPersonCharacter character;
    public Animator animator;
    public bool followPlayer;
    public bool isonfloor;
    private LineRenderer lineRenderer;
    private bool pathDrawn;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        agent = GetComponent<NavMeshAgent>();
        character = GetComponent<ThirdPersonCharacter>();
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();

        // Initialize states of variables
        agent.updateRotation = false;
        followPlayer = false;
        pathDrawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Set destination only if the current path has been calculated
        if (!agent.pathPending)                                      
        {
            // Draw path if not already drawn
            if(!pathDrawn)
            {
                DrawPath(agent.path);
                pathDrawn = true;
            }

            // Go to the point where ray cast hits 
            if (Input.GetMouseButtonDown(0) && !followPlayer)    
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    // Reset the path so that the agent moves to the new destination
                    agent.ResetPath();

                    // Agent follows the player
                    agent.SetDestination(hit.point);

                    // Set pathDrawn to false
                    pathDrawn = false;
                }
            }

            // Follow the player when left mouse button is clicked
            if (followPlayer)                                        
            {
                // Reset the path so that the agent moves to the new destination
                //agent.ResetPath();

                // Agent follows the player
                agent.SetDestination(Camera.main.transform.position);

                // Set pathDrawn to false
                pathDrawn = false;
            }

            // Follow player only when F is pressed
            if (Input.GetKeyDown(KeyCode.F) && !followPlayer)       
            {
                // Reset the path so that the agent moves to the new destination
                agent.ResetPath();

                // Agent follows the player
                agent.SetDestination(Camera.main.transform.position);

                // Set pathDrawn to false
                pathDrawn = false;
            }
        }

        // The object moves due to its animation to positions determined by the navmesh agent
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);

    }

    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) //if the path has 1 or no corners, there is no need
            return;

        lineRenderer.positionCount = path.corners.Length; //set the array of positions to the amount of corners

        lineRenderer.SetPosition(0, transform.position); //set the first point to the current position of the GameObject

        for (var i = 1; i < path.corners.Length; i++)
        {
            lineRenderer.SetPosition(i, path.corners[i]); //go through each corner and set that to the line renderer's position
        }
    }
}
