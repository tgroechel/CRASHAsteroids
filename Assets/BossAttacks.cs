using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class BossAttacks : MonoBehaviour {

        public enum FirePatterns { Single, OffsetSingle, HorizontalLines, VerticalLines, Circle, Cross, Die, Laser, LaserAbort };
        public enum GunsToUse { Left, Right, Both };

        public float rotationSpeed = 1;
        public bool rotatingToPlayer;

        public GameObject leftGunGO;
        public GameObject rightGunGO;
        public GameObject laserGO;

        private BossAttackScript leftGun;
        private BossAttackScript rightGun;
        private BossLaserScript laser;

        void Awake() {
            leftGun = leftGunGO.GetComponent<BossAttackScript>();
            rightGun = rightGunGO.GetComponent<BossAttackScript>();
            laser = laserGO.GetComponent<BossLaserScript>();
        }

        // Update is called once per frame
        void Update() {
            // rotate to player
            GameObject MountTop = GameObject.Find("Mount_Top_Hvy");
            Vector3 relativePos2Player = Camera.main.transform.position - MountTop.transform.position;
            relativePos2Player.y = 0;
            Quaternion lookAtPlayerRotation = Quaternion.identity;
            if (relativePos2Player != Vector3.zero) {
                lookAtPlayerRotation = Quaternion.LookRotation(relativePos2Player, Vector3.up);
            }

            if (rotatingToPlayer) {
                if (Mathf.Abs(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y) < 180) {
                    float sign = Mathf.Sign(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y);
                    if (Mathf.Abs(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y) > 30)
                        MountTop.transform.rotation *= Quaternion.Euler(0, sign * 4 * rotationSpeed, 0);
                    else if (Mathf.Abs(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y) > rotationSpeed)
                        MountTop.transform.rotation *= Quaternion.Euler(0, sign * rotationSpeed, 0);
                }
                else {
                    float sign = Mathf.Sign(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y);
                    if (Mathf.Abs(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y) > 30)
                        MountTop.transform.rotation *= Quaternion.Euler(0, -sign * 4 * rotationSpeed, 0);
                    else if (Mathf.Abs(lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y) > rotationSpeed)
                        MountTop.transform.rotation *= Quaternion.Euler(0, -sign * rotationSpeed, 0);
                }
            }


            /*
            if (lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y > 30)
                MountTop.transform.rotation *= Quaternion.Euler(0, 5 * rotationSpeed, 0);
            else if (lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y < -30)
                MountTop.transform.rotation *= Quaternion.Euler(0, -5 * rotationSpeed, 0);
            if (lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y > 1)
                MountTop.transform.rotation *= Quaternion.Euler(0, rotationSpeed, 0);
            else if (lookAtPlayerRotation.eulerAngles.y - MountTop.transform.rotation.eulerAngles.y < -1)
                MountTop.transform.rotation *= Quaternion.Euler(0, - rotationSpeed, 0);
                */
            //MountTop.transform.rotation = lookAtPlayerRotation.normalized;



            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Pattern0(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Pattern1(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Pattern2(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                Pattern3(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                CircularPattern(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                CrossPattern(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7)) {
                DiePattern(GunsToUse.Both);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8)) {
                Laser();
            }
        }

        public void PatternSelector(FirePatterns pattern, GunsToUse guns) {
            switch (pattern) {
                case FirePatterns.Single:
                    Pattern0(guns);
                    break;
                case FirePatterns.OffsetSingle:
                    Pattern1(guns);
                    break;
                case FirePatterns.HorizontalLines:
                    Pattern2(guns);
                    break;
                case FirePatterns.VerticalLines:
                    Pattern3(guns);
                    break;
                case FirePatterns.Circle:
                    CircularPattern(guns);
                    break;
                case FirePatterns.Cross:
                    CrossPattern(guns);
                    break;
                case FirePatterns.Die:
                    DiePattern(guns);
                    break;
                case FirePatterns.Laser:
                    Laser();
                    break;
                case FirePatterns.LaserAbort:
                    AbortLaser();
                    break;
                default:
                    break;
            }

        }

        // just a single boring bullet
        public void Pattern0(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.Pattern0();
            else if (guns == GunsToUse.Right)
                rightGun.Pattern0();
            else {
                leftGun.Pattern0();
                rightGun.Pattern0();
            }
        }

        // one bullet with some random offeset
        public void Pattern1(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.Pattern1(Random.Range(0, 1000));
            else if (guns == GunsToUse.Right)
                rightGun.Pattern1(Random.Range(0, 1000));
            else {
                leftGun.Pattern1(Random.Range(0, 1000));
                rightGun.Pattern1(Random.Range(0, 1000));
            }
        }

        // an array of bullets - horizontal
        public void Pattern2(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.Pattern2(Random.Range(0, 1000));
            else if (guns == GunsToUse.Right)
                rightGun.Pattern2(Random.Range(0, 1000));
            else {
                leftGun.Pattern2(Random.Range(0, 1000));
                rightGun.Pattern2(Random.Range(0, 1000));
            }
        }

        // an array of bullets but slightly different - vertical
        public void Pattern3(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.Pattern3(Random.Range(0, 1000));
            else if (guns == GunsToUse.Right)
                rightGun.Pattern3(Random.Range(0, 1000));
            else {
                leftGun.Pattern3(Random.Range(0, 1000));
                rightGun.Pattern3(Random.Range(0, 1000));
            }
        }

        // a circle
        public void CircularPattern(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.CircularPattern();
            else if (guns == GunsToUse.Right)
                rightGun.CircularPattern();
            else {
                leftGun.CircularPattern();
                rightGun.CircularPattern();
            }
        }

        // a cross
        public void CrossPattern(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.CrossPattern();
            else if (guns == GunsToUse.Right)
                rightGun.CrossPattern();
            else {
                leftGun.CrossPattern();
                rightGun.CrossPattern();
            }
        }

        // die b**** !
        public void DiePattern(GunsToUse guns) {
            if (guns == GunsToUse.Left)
                leftGun.DiePattern();
            else if (guns == GunsToUse.Right)
                rightGun.DiePattern();
            else {
                leftGun.DiePattern();
                rightGun.DiePattern();
            }
        }

        // laser!
        public void Laser() {
            laser.startLaser();
        }
        public void AbortLaser() {
            laser.stop();
        }
    }
}