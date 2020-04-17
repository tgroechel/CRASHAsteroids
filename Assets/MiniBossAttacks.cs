using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash
{
    public class MiniBossAttacks : MonoBehaviour
    {
        private BossAttackScript gun;
        void Start()
        {
            gun = GameObject.Find("Barrel_End_1").GetComponent<BossAttackScript>();
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
