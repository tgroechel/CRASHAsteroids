using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class MoveTowards : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("The agent has arrived when the destination is less than the specified amount. This distance should be greater than or equal to the NavMeshAgent StoppingDistance.")]
        public SharedFloat arriveDistance = 0.2f;
        [Tooltip("The distance you want to move relative to your current position.")]
        public SharedVector3 relativePositionChange;
        [Tooltip("The target position the agent is moving towards. Only used if relative position change is 0.")]
        public SharedVector3 targetPosition;

        private Vector3 goalPosition;

        public override void OnStart()
        {
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

            return TaskStatus.Running;
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        private bool HasArrived()
        {
            return (goalPosition - transform.position).sqrMagnitude <= Mathf.Pow(arriveDistance.Value,2);
        }
    }
}