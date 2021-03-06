﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Crash {
    public class BossAttackScript : MonoBehaviour {
        public float bulletSpeed = 2;
        public int bulletPoolSize;
        public GameObject explodePrefab;
        public AudioClip explodeSFX;

        public GameObject gun;

        private Animator animator;
        private bool alive = true;

        private GameObject effect;
        private GameObject fireEffect;

        public GameObject barrel1;
        public GameObject barrel2;

        float timeElapsed;

        void Start() {
            effect = Resources.Load<GameObject>(ResourcePathManager.bossProjectile) as GameObject;
            fireEffect = Resources.Load<GameObject>(ResourcePathManager.bossGunFire) as GameObject;
            GetComponent<BulletPoolManager>().createPool("boss bullet", effect, bulletPoolSize);
            animator = GetComponent<Animator>();
        }

        void Update() {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > 1) {
                //gun.GetComponent<Animator>().ResetTrigger("Fold");
                //gun.GetComponent<Animator>().ResetTrigger("Unfold");
                //gun.GetComponent<Animator>().SetTrigger("Shoot");
                GetComponent<Animator>().enabled = false;
                //Debug.Log("shooooot!");
            }
        }

        private void gunFire(Vector3 spawnPos) {
            GameObject fire = Instantiate(fireEffect, spawnPos, Quaternion.identity);
        }

        private GameObject getBullet() {
            timeElapsed = 0;
            gun.GetComponent<Animator>().enabled = true;


            GameObject vfx = GetComponent<BulletPoolManager>().getInstance("boss bullet");
            if (vfx != null)
                vfx.GetComponent<ProjectileMoveScript2>().speed = bulletSpeed;
            return vfx;
        }

        private Vector3 getSpawnPos() {
            if (Random.Range(-10, 10) < 0)
                return barrel1.transform.position;
            else
                return barrel2.transform.position;
        }

        public void Pattern0() {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            GameObject vfx = getBullet();
            if (vfx != null) {
                vfx.transform.position = spawnPos;
                vfx.transform.rotation = Quaternion.identity;
                Vector3 dir = Camera.main.transform.position - transform.position;
                vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
            }

        }

        public void Pattern1(int i) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            int ii = i % 18;
            if (ii >= 9)
                ii = 17 - ii;

            GameObject vfx = getBullet();
            if (vfx != null) {
                vfx.transform.position = spawnPos;// + new Vector3(0, 0.1f, 0);
                vfx.transform.rotation = Quaternion.identity;
                Vector3 targetPos = Camera.main.transform.position;
                targetPos.y += Mathf.Cos(i * 37f / 360 * Mathf.PI) * 0.1f;
                Vector3 dir = Quaternion.AngleAxis(-16 + 4 * ii, Vector3.up) * (targetPos - transform.position);
                vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
            }

        }



        public void Pattern2(int i) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            int ii = i % 3;
            GameObject vfx;
            List<GameObject> bullets = new List<GameObject>();

            for (int jj = 0; jj < 7; jj++) {
                vfx = getBullet();
                if (vfx == null) continue;
                vfx.transform.position = spawnPos;// + new Vector3(0, jj * 0.05f, 0);
                vfx.transform.rotation = Quaternion.identity;
                bullets.Add(vfx);
            }
            int j = 0;
            Vector3 targetPos, dir;
            foreach (GameObject bullet in bullets) {
                targetPos = Camera.main.transform.position;
                if (ii == 0)
                    targetPos.y += 0.15f;
                if (ii == 2)
                    targetPos.y -= 0.15f;
                dir = Quaternion.AngleAxis(-12 + 4 * j, Vector3.up) * (targetPos - transform.position);
                bullet.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                j++;
            }
            if (bullets[0] != null)
                bullets[0].GetComponent<ProjectileMoveScript2>().playSpawnSound();
        }

        public void Pattern3(int i) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            int ii = i % 3;
            GameObject vfx;
            List<GameObject> bullets = new List<GameObject>();

            for (int jj = 0; jj < 7; jj++) {
                GameObject head = Camera.main.gameObject;
                Vector3 relativePos2Player = head.transform.position - transform.position;
                relativePos2Player.y = 0;
                relativePos2Player.Normalize();
                relativePos2Player.x *= 0.05f * jj;
                relativePos2Player.z *= 0.05f * jj;

                vfx = getBullet();
                if (vfx == null) continue;
                vfx.transform.position = spawnPos;// + relativePos2Player;
                vfx.transform.rotation = Quaternion.identity;
                bullets.Add(vfx);
            }
            int j = 0;
            Vector3 targetPos, dir;
            foreach (GameObject bullet in bullets) {
                targetPos = Camera.main.transform.position;
                dir = Quaternion.AngleAxis(-7.5f + 2.5f * j, Vector3.forward) * (targetPos - transform.position);
                bullet.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                j++;
            }
            if (bullets[0] != null)
                bullets[0].GetComponent<ProjectileMoveScript2>().playSpawnSound();
        }

        public void CircularPattern(float offset = 0.2f) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            GameObject vfx;
            List<GameObject> bullets = new List<GameObject>();

            bool soundPlayed = false;


            offset = 0.3f;
            for (int jj = 0; jj < 8; jj++) {
                vfx = getBullet();
                if (vfx == null) continue;

                vfx.transform.position = spawnPos;
                vfx.transform.rotation = Quaternion.identity;

                GameObject head = Camera.main.gameObject;
                Vector3 relativePos2Player = head.transform.position - transform.position;

                Quaternion relativeRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), relativePos2Player.normalized);
                Vector3 newY = relativeRotation * new Vector3(0, 1, 0);
                Vector3 newZ = relativeRotation * new Vector3(0, 0, 1);

                float angle = 2 * (float)Math.PI / 8 * jj;
                Vector3 dir = head.transform.position + newY * (float)Math.Cos(angle) * offset
                    + newZ * (float)Math.Sin(angle) * offset - transform.position;
                vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                if (!soundPlayed) {
                    vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                    soundPlayed = true;
                }
            }
        }

        public void CrossPattern(float offset = 0.3f) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            GameObject vfx;
            List<GameObject> bullets = new List<GameObject>();

            bool soundPlayed = false;

            float[] yOffsets = { -2, -1, 0, 1, 2, 0, 0, 0, 0 };
            float[] zOffsets = { 0, 0, 0, 0, 0, -2, -1, 1, 2 };

            for (int jj = 0; jj < 9; jj++) {
                vfx = getBullet();
                if (vfx == null) continue;

                vfx.transform.position = spawnPos;
                vfx.transform.rotation = Quaternion.identity;

                GameObject head = Camera.main.gameObject;
                Vector3 relativePos2Player = head.transform.position - transform.position;

                Quaternion relativeRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), relativePos2Player.normalized);
                Vector3 newY = relativeRotation * new Vector3(0, 1, 0);
                Vector3 newZ = relativeRotation * new Vector3(0, 0, 1);

                Vector3 dir = head.transform.position + newY * yOffsets[jj] * offset
                    + newZ * zOffsets[jj] * offset - transform.position;
                vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                if (!soundPlayed) {
                    vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                    soundPlayed = true;
                }
            }
        }

        public void DiePattern(float offset = 0.15f) {
            Vector3 spawnPos = getSpawnPos();
            gunFire(spawnPos);
            GameObject vfx;
            List<GameObject> bullets = new List<GameObject>();

            bool soundPlayed = false;

            float[] yOffsets = { -2, -2, -1, 0 , 1, 2, 2, 1, 0, -1,
                -2, -2, -2, -1, 0, 1, 2, 2, 2,
                -2, -2, -2, -1, 0, 0, 1, 2, 2, 2};
            float[] zOffsets = { -5, -4, -3, -3, -3, -4, -5, -5, -5, -5,
                -1, 0, 1, 0, 0, 0, -1, 0, 1,
                3, 4, 5, 3, 3, 4, 3, 3, 4, 5};

            for (int jj = 0; jj < 29; jj++) {
                vfx = getBullet();
                if (vfx == null) continue;

                vfx.transform.position = spawnPos;
                vfx.transform.rotation = Quaternion.identity;

                GameObject head = Camera.main.gameObject;
                Vector3 relativePos2Player = head.transform.position - transform.position;

                Quaternion relativeRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), relativePos2Player.normalized);
                Vector3 newY = relativeRotation * new Vector3(0, 1, 0);
                Vector3 newZ = relativeRotation * new Vector3(0, 0, 1);

                Vector3 dir = head.transform.position + newY * yOffsets[jj] * offset
                    + newZ * zOffsets[jj] * offset - transform.position;
                vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
                if (!soundPlayed) {
                    vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                    soundPlayed = true;
                }
            }
        }
    }
}