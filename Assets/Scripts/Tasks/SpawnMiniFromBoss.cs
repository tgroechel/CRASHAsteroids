using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Crash
{
    public class SpawnMiniFromBoss : Action
    {
        public Object lastSpawnedMinion;
        private MinionSpawner minionSpawner;

        public override void OnAwake()
        {
            minionSpawner = GetComponent<MinionSpawner>();
        }

        public override void OnStart()
        {
            lastSpawnedMinion = minionSpawner.SpawnMinion();
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
