using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash
{
    public class MiniBossAttacks : MonoBehaviour
    {
        public enum FirePatterns { Single, Circle, Cross};

        public GameObject gunGO;

        public GameObject top;

        public bool rotatingToPlayer;

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






            Vector3 relativePos2Player = Camera.main.transform.position - gunGO.transform.position;
            Quaternion lookAtPlayerRotation = Quaternion.identity;
            if (relativePos2Player != Vector3.zero)
            {
                lookAtPlayerRotation = Quaternion.LookRotation(relativePos2Player, gameObject.transform.up);
            }

            Vector3 relativePos2Player2 = Camera.main.transform.position - top.transform.position;
            Quaternion lookAtPlayerRotation2 = Quaternion.identity;
            if (relativePos2Player2 != Vector3.zero)
            {
                lookAtPlayerRotation2 = Quaternion.LookRotation(relativePos2Player2, gameObject.transform.up);
            }

            if (rotatingToPlayer)
            {

                //top.transform.localRotation = Quaternion.Euler(0, lookAtPlayerRotation2.eulerAngles.y - transform.rotation.eulerAngles.y, 0);// Quaternion.Euler(gameObject.transform.eulerAngles.x, lookAtPlayerRotation2.eulerAngles.y, gameObject.transform.eulerAngles.z);// * gameObject.transform.rotation * Quaternion.Euler(0,180,0);
                top.transform.rotation = lookAtPlayerRotation2;
                float xxx = top.transform.localRotation.eulerAngles.x;
                if (xxx > 180)
                    xxx -= 360;
                if (Mathf.Abs(xxx) > 20)
                    xxx = 20 * Mathf.Sign(xxx);
                float zzz = top.transform.localRotation.eulerAngles.z;
                if (zzz > 180)
                    zzz -= 360;
                if (Mathf.Abs(zzz) > 20)
                    zzz = 20 * Mathf.Sign(zzz);

                top.transform.localRotation = Quaternion.Euler(
                    xxx,
                    top.transform.localRotation.eulerAngles.y,
                    zzz);

                gunGO.transform.rotation = lookAtPlayerRotation;// Quaternion.Euler(lookAtPlayerRotation.eulerAngles.x, top.transform.rotation.eulerAngles.y, lookAtPlayerRotation.eulerAngles.z);
                float yyy = gunGO.transform.localRotation.eulerAngles.y;
                if (yyy > 180)
                    yyy -= 360;
                if (Mathf.Abs(yyy) > 40)
                    yyy = 40 * Mathf.Sign(yyy);
                
                gunGO.transform.localRotation = Quaternion.Euler(
                    gunGO.transform.localRotation.eulerAngles.x,
                    yyy,
                    gunGO.transform.localRotation.eulerAngles.z);
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
