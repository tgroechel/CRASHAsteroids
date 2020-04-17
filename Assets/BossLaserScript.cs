using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class BossLaserScript : MonoBehaviour {
        private GameObject effect;
        private GameObject aimLaser;
        private GameObject laser;
        private GameObject laserGun;
        private GameObject laserGunMount;
        private EGA_Laser laserScript;
        private float timeElapsed;

        private const int LaserGunRotationLimitX = 90;
        private const int LaserGunRotationLimitY = 15;
        private const int LaserGunRotationLimitZ = 0;

        private bool started;
        private bool laserShot;
        private bool finalChargeStarted;
        private Vector3 shootHere;

        public AudioClip shotSFX;
        public AudioClip chargeSFX;
        public AudioClip finalChargeSFX;
        public float gunRotationSpeed;

        void Start() {
            effect = Resources.Load<GameObject>(ResourcePathManager.bossLaser) as GameObject;
            //laserGun = transform.parent.gameObject;
            //laserGunMount = laserGun.transform.parent.gameObject;
            //aimLaser = transform.GetChild(0).gameObject; //for using the FX game object

            GameObject aimLaserEffect = Resources.Load<GameObject>(ResourcePathManager.aimLaser) as GameObject;
            aimLaser = Instantiate(aimLaserEffect, transform.position, transform.rotation);
            aimLaser.SetActive(false);
            started = false;
        }

        void Update() {
            timeElapsed += Time.deltaTime;
            if (started) {
                if (timeElapsed < 5) {
                    //method0 - just rotate the laser instead of the entire gun; looks a bit unrealistic
                    Vector3 direction = Camera.main.transform.position - new Vector3(0, 0, 0.02f) - aimLaser.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    aimLaser.transform.localRotation = Quaternion.Lerp(aimLaser.transform.rotation, rotation, 1);

                    //method1 -- depending on localEulerAngles may cause issues since quaternions are underlying.
                    //Vector3 direction = laserGun.transform.InverseTransformPoint(Camera.main.transform.position) - laserGun.transform.localPosition - new Vector3(0, 0, 0.02f);
                    //direction.z = 0;
                    //direction = direction / direction.magnitude;
                    //Quaternion rotation = Quaternion.LookRotation(direction, laserGunMount.transform.up);//* Quaternion.Euler(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    //rotation = RotateLimited(Quaternion.RotateTowards(laserGun.transform.localRotation, rotation, Time.deltaTime * gunRotationSpeed));
                    //laserGun.transform.localEulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, 0); //constrained z method

                    //method2 -- depending on localEulerAngles may cause issues since quaternions are underlying.
                    //laserGun.transform.LookAt(Camera.main.transform, laserGunMount.transform.up);
                    //laserGun.transform.localEulerAngles = RotateLimited(laserGun.transform.localEulerAngles);

                    //method3 - difficult to implement angle limits
                    //Vector3 direction = (Camera.main.transform.position - laserGun.transform.position).normalized;
                    //float angleDif = Vector3.SignedAngle(laserGun.transform.forward, direction, Vector3.up);
                    //print(angleDif);
                    //print(laserGun.transform.localEulerAngles);
                    //if (angleDif > 5f)
                    //{
                    //    laserGun.transform.Rotate(new Vector3(-1f, 0f));
                    //}
                    //else if (angleDif < -5)
                    //{
                    //    laserGun.transform.Rotate(new Vector3(1f, 0f));
                    //}
                }
                else if (timeElapsed < 6) {
                    if (!finalChargeStarted) {
                        if (GetComponent<AudioSource>()) {
                            GetComponent<AudioSource>().Stop();
                        }
                        if (finalChargeSFX != null && GetComponent<AudioSource>()) {
                            GetComponent<AudioSource>().PlayOneShot(finalChargeSFX);
                        }
                        finalChargeStarted = true;
                        shootHere = Camera.main.transform.position;
                    }
                }
                else {
                    if (!laserShot) {
                        shoot();
                    }
                    else {
                        RaycastHit hit;
                        Physics.Raycast(laser.transform.position, laser.transform.TransformDirection(Vector3.forward), out hit, 30f);

                        if (hit.collider && hit.collider.gameObject.GetComponent<PlayerCollision>()) {
                            Camera.main.gameObject.GetComponent<CameraShakeSimpleScript>()?.ShakeCamera();
                        }

                        if (timeElapsed > 8f)
                            stop();
                    }
                }
            }
        }

        //Applies rotation limits to a rotation quaternion.
        private Quaternion RotateLimited(Quaternion rotation)
        {
            Vector3 eulerRot = rotation.eulerAngles;
            eulerRot.x = Mathf.Abs(eulerRot.x) > LaserGunRotationLimitX ? Mathf.Sign(eulerRot.x) * LaserGunRotationLimitX : eulerRot.x;
            eulerRot.y = Mathf.Abs(eulerRot.y) > LaserGunRotationLimitY ? Mathf.Sign(eulerRot.y) * LaserGunRotationLimitY : eulerRot.y;
            eulerRot.z = Mathf.Abs(eulerRot.z) > LaserGunRotationLimitZ ? Mathf.Sign(eulerRot.z) * LaserGunRotationLimitZ : eulerRot.z;
            return Quaternion.Euler(eulerRot);
        }

        //Applies rotation limits to a rotation vector
        private Vector3 RotateLimited(Vector3 angles)
        {
            angles.x = Mathf.Abs(angles.x) > LaserGunRotationLimitX ? Mathf.Sign(angles.x) * LaserGunRotationLimitX : angles.x;
            angles.y = Mathf.Abs(angles.y) > LaserGunRotationLimitY ? Mathf.Sign(angles.y) * LaserGunRotationLimitY : angles.y;
            angles.z = Mathf.Abs(angles.z) > LaserGunRotationLimitZ ? Mathf.Sign(angles.z) * LaserGunRotationLimitZ : angles.z;
            return angles;
        }

        private void shoot() {
            aimLaser.SetActive(false);
            if (GetComponent<AudioSource>()) {
                GetComponent<AudioSource>().Stop();
            }
            if (shotSFX != null && GetComponent<AudioSource>()) {
                GetComponent<AudioSource>().PlayOneShot(shotSFX);
            }
            Destroy(laser);
            laser = Instantiate(effect, transform.position, transform.rotation);
            Vector3 direction = shootHere - laser.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            laser.transform.localRotation = Quaternion.Lerp(laser.transform.rotation, rotation, 1);
            //laserGun.transform.localRotation = Quaternion.Lerp(laserGun.transform.rotation, rotation, 1);
            laserScript = laser.GetComponent<EGA_Laser>();
            laserShot = true;
        }


        public void startLaser()
        {
            if (started)
                return;
            started = true;
            laserShot = false;
            finalChargeStarted = false;
            aimLaser.SetActive(true);
            aimLaser.transform.position = transform.position + new Vector3(0, 0, 0.3f);
            timeElapsed = 0;
            if (chargeSFX != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(chargeSFX);
            }
        }

        public void stop() {
            laserScript.DisablePrepare();
            Destroy(laser, 1);
            started = false;
        }
    }
}