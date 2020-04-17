using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace Crash
{
    public class FireBossWeapon : Action
    {
        public BossAttacks.FirePatterns chosenPattern;
        public BossAttacks.GunsToUse gunsToUse;

        private BossAttacks bossAttacks;

        public override void OnAwake()
        {
            bossAttacks = GetComponent<BossAttacks>();
        }

        public override void OnStart()
        {
            bossAttacks.PatternSelector(chosenPattern, gunsToUse);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }
    }
}
