using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Crash
{
    public class FireMiniWeapon : Action
    {
        public MiniBossAttacks.FirePatterns chosenPattern;

        private MiniBossAttacks bossAttacks;

        public override void OnAwake()
        {
            bossAttacks = GetComponent<MiniBossAttacks>();
        }

        public override void OnStart()
        {
            bossAttacks.PatternSelector(chosenPattern);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}

