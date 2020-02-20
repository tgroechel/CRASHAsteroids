using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class MoveToward : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("The agent has arrived when the destination is less than the specified amount. This distance should be greater than or equal to the NavMeshAgent StoppingDistance.")]
        public SharedFloat arriveDistance = 0.2f;
        [Tooltip("The target position the agent is moving towards")]
        public SharedVector3 targetPosition;

        private Rigidbody myBody;

        public override void OnStart()
        {
            myBody = GetComponent<Rigidbody>();
            //Vector3 direction = (targetPosition.Value - myBody.transform.position).normalized * speed.Value;
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (HasArrived())
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Running;
        }

        /// <summary>
        /// Has the agent arrived at the destination?
        /// </summary>
        /// <returns>True if the agent has arrived at the destination.</returns>
        private bool HasArrived()
        {
            return (targetPosition.Value - myBody.transform.position).sqrMagnitude <= Mathf.Pow(arriveDistance.Value,2);
        }


        /// <summary>
        /// The behavior tree has ended. Stop moving.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            //Stop();
        }
    }
}