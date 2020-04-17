using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Crash
{
    public class EventTest : MonoBehaviour
    {
        public BehaviorTree behaviorTree;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           if(Input.GetMouseButtonDown(0)) 
            behaviorTree.SendEvent<object>("Test", 0);
        }
    }
}
