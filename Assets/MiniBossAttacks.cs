using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash
{
    public class MiniBossAttacks : MonoBehaviour
    {
        public enum FirePatterns { Single, Circle, Cross};

        public GameObject gunGO;

        private BossAttackScript gun;
        void Start()
        {
            gun = gunGO.GetComponent<BossAttackScript>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Pattern0();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                CircularPattern();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                CrossPattern();
            }
        }

        public void PatternSelector(FirePatterns pattern)
        {
            switch (pattern)
            {
                case FirePatterns.Single:
                    Pattern0();
                    break;
                case FirePatterns.Circle:
                    CircularPattern();
                    break;
                case FirePatterns.Cross:
                    CrossPattern();
                    break;
                default:
                    break;
            }

        }

        public void Pattern0()
        {
            gun.Pattern0();
        }

        public void CircularPattern()
        {
            gun.CircularPattern();
        }

        public void CrossPattern()
        {
            gun.CrossPattern();
        }
    }
}
