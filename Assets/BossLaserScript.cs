using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class BossLaserScript : MonoBehaviour {
        private GameObject effect;
        private GameObject aimLaser;
        private GameObject laser;
        private GameObject laserGun;
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
            //GameObject aimLaserEffect = Resources.Load<GameObject>(ResourcePathManager.aimLaser) as GameObject;
            laserGun = transform.parent.parent.parent.gameObject; //TODO change this to get rotation platform in a better way.
            //aimLaser = Instantiate(aimLaserEffect, transform.position, transform.rotation);
            aimLaser = transform.GetChild(0).gameObject;
            aimLaser.SetActive(false);
            
            started = false;
        }

        void Update() {
            timeElapsed += Time.deltaTime;
            if (started) {
                if (timeElapsed < 5) {
                    //Vector3 direction = Camera.main.transform.position - new Vector3(0, 0, 0.02f) - aimLaser.transform.position;
                    //Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    //aimLaser.transform.localRotation = Quaternion.Lerp(aimLaser.transform.rotation, rotation, 1);
                    Vector3 direction = laserGun.transform.InverseTransformPoint(Camera.main.transform.position) - laserGun.transform.localPosition - new Vector3(0, 0, 0.02f);
                    direction = direction / direction.magnitude;
                    Quaternion rotation = Quaternion.LookRotation(direction,laserGun.transform.up);//* Quaternion.Euler(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    rotation = Quaternion.RotateTowards(laserGun.transform.localRotation, rotation, Time.deltaTime * gunRotationSpeed);
                    laserGun.transform.rotation = rotation;
                    //laserGun.transform.localEulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, 0); //constrained z method
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

        //Applies rotation limits to a rotation vector.
        private Quaternion RotateLimited(Quaternion rotation)
        {
            Vector3 eulerRot = rotation.eulerAngles;
            float x = Mathf.Abs(eulerRot.x) > LaserGunRotationLimitX ? Mathf.Sign(eulerRot.x) * LaserGunRotationLimitX : eulerRot.x;
            float y = Mathf.Abs(eulerRot.y) > LaserGunRotationLimitY ? Mathf.Sign(eulerRot.y) * LaserGunRotationLimitY : eulerRot.y;
            float z = Mathf.Abs(eulerRot.z) > LaserGunRotationLimitZ ? Mathf.Sign(eulerRot.z) * LaserGunRotationLimitZ : eulerRot.z;
            return Quaternion.Euler(eulerRot);
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