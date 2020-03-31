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

    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        followPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !followPlayer)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        if(followPlayer)
        {
            // Agent follows the player
            agent.SetDestination(Camera.main.transform.position);
        }

        // The object moves due to its animation to positions determined by the navmesh agent
        if (agent.remainingDistance > agent.stoppingDistance)
            character.Move(agent.desiredVelocity, false, false);
        else
            character.Move(Vector3.zero, false, false);

    }
}
