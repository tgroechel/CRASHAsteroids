using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class MoveTowardsOnGround : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("The agent has arrived when the destination is less than the specified amount. This distance should be greater than or equal to the NavMeshAgent StoppingDistance.")]
        public SharedFloat arriveDistance = 0.2f;
        [Tooltip("The distance you want to move relative to your current position.")]
        public SharedVector3 relativePositionChange;
        [Tooltip("The target position the agent is moving towards. Only used if relative position change is 0.")]
        public SharedVector3 targetPosition;
        [Tooltip("The distance of the center of the robot to the ground, used for placing robot on the ground.")]
        public SharedFloat groundOffset = 1.0f;


        private Vector3 goalPosition;

        public override void OnStart()
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up * -1.0f, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Floor")
                {
                    transform.position = hit.point + hit.normal.normalized * groundOffset.Value;
                    transform.rotation.SetFromToRotation(transform.up, hit.normal);
                }
            }

            if (relativePositionChange.Value == new Vector3(0f, 0f, 0f))
            {
                goalPosition = targetPosition.Value;
            }
            else
            {
                goalPosition = transform.position + relativePositionChange.Value;
            }
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (HasArrived())
            {
                return TaskStatus.Success;
            }
            transform.position = Vector3.MoveTowards(transform.position, goalPosition, speed.Value * Time.deltaTime);
            StayOnGround();

            return TaskStatus.Running;
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        private bool HasArrived()
        {
            return Mathf.Pow((goalPosition.x - transform.position.x),2) + Mathf.Pow((goalPosition.z - transform.position.z), 2) <= Mathf.Pow(arriveDistance.Value, 2);
        }

        /// <summary>
        /// Adjust Y cordinates and rotation to make robot appears like staying on ground
        /// </summary>
        private void StayOnGround()
        {
            // Do a raycast from center of body, oriented downwards in order to 
            // find floor object and determine robot's exact position and rotation 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up * -1.0f, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "Floor")
                {
                    Vector3 newPos = transform.position;
                    newPos.y = hit.point.y + hit.normal.normalized.y * groundOffset.Value;
                    transform.position = newPos;
                    transform.rotation.SetFromToRotation(transform.up, hit.normal);
                }
            }
        }
    }
}