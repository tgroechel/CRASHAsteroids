using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAttackScript : MonoBehaviour {

    public float waitDuration = 1f;
    public float bulletSpeed = 2;
    public int bulletPoolSize;
    public GameObject explodePrefab;
    public AudioClip explodeSFX;

    private Animator animator;
    private bool alive = true;

    private GameObject effect;

    void Start() {
        effect = Resources.Load<GameObject>(ResourcePathManager.bossProjectile) as GameObject;
        GetComponent<BulletPoolManager>().createPool("boss bullet", effect, bulletPoolSize);
        StartCoroutine("ShootPlayer");
        animator = GetComponent<Animator>();
    }

    void Update() {
        /*
        Vector3 relativePos2Player = Camera.main.transform.position - transform.position;
        relativePos2Player.y = 0;
        Quaternion lookAtPlayerRotation = Quaternion.identity;
        if (relativePos2Player != Vector3.zero) {
            lookAtPlayerRotation = Quaternion.LookRotation(relativePos2Player, Vector3.up);
        }
        transform.rotation = lookAtPlayerRotation;

        if (Input.GetKeyDown(KeyCode.P)) {
            animator.SetTrigger("Die");
            alive = false;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<BossLaserScript>().startLaser();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            CrossPattern();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            CircularPattern();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            DiePattern();
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (explodeSFX != null && GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().PlayOneShot(explodeSFX);
            }
            for (int i = 0; i < 5; i++)
            {
                if (explodePrefab != null)
                {

                    var hitVFX = Instantiate(explodePrefab, transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0, 0.2f), Random.Range(-0.3f, 0.3f)), Quaternion.identity) as GameObject;
                    hitVFX.GetComponent<ProjectileMoveScript>().speed = 0f;
                    var ps = hitVFX.GetComponent<ParticleSystem>();
                    if (ps == null)
                    {
                        var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitVFX, psChild.main.duration);
                    }
                    else
                        Destroy(hitVFX, ps.main.duration);
                }
            }


            Destroy(gameObject, 0.3f);
        }*/
    }

    IEnumerator ShootPlayer() {
        yield return new WaitForSeconds(1f);
        /*
        int i = 0;
        while (alive) {
            if (i % 13 == 0) {
                if (i % 2 == 0)
                    Pattern3(i);
                else
                    Pattern2(i);
            }
            else
                Pattern1(i);
            i++;
            yield return new WaitForSeconds(waitDuration);
        }*/
    }

    private GameObject getBullet() {
        GameObject vfx = GetComponent<BulletPoolManager>().getInstance("boss bullet");
        if (vfx != null)
            vfx.GetComponent<ProjectileMoveScript2>().speed = bulletSpeed;
        return vfx;
    }

    public void Pattern0()
    {
        GameObject vfx = getBullet();
        if (vfx != null)
        {
            vfx.transform.position = transform.position;
            vfx.transform.rotation = Quaternion.identity;
            Vector3 dir = Camera.main.transform.position - transform.position;
            vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
            vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
        }

    }

    public void Pattern1(int i) {
        int ii = i % 18;
        if (ii >= 9)
            ii = 17 - ii;

        GameObject vfx = getBullet();
        if (vfx != null)
        {
            vfx.transform.position = transform.position;// + new Vector3(0, 0.1f, 0);
            vfx.transform.rotation = Quaternion.identity;
            Vector3 targetPos = Camera.main.transform.position;
            targetPos.y += Mathf.Cos(i * 37f / 360 * Mathf.PI) * 0.1f;
            Vector3 dir = Quaternion.AngleAxis(-16 + 4 * ii, Vector3.up) * (targetPos - transform.position);
            vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
            vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
        }
        
    }



    public void Pattern2(int i) {
        int ii = i % 3;
        GameObject vfx;
        List<GameObject> bullets = new List<GameObject>();

        for (int jj = 0; jj < 7; jj++) {
            vfx = getBullet();
            if (vfx == null) continue;
            vfx.transform.position = transform.position;// + new Vector3(0, jj * 0.05f, 0);
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
        if (bullets[0]!= null)
            bullets[0].GetComponent<ProjectileMoveScript2>().playSpawnSound();
    }

    public void Pattern3(int i) {
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
            vfx.transform.position = transform.position;// + relativePos2Player;
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
        GameObject vfx;
        List<GameObject> bullets = new List<GameObject>();

        bool soundPlayed = false;


        offset = 0.3f;
        for (int jj = 0; jj < 8; jj++)
        {
            vfx = getBullet();
            if (vfx == null) continue;

            vfx.transform.position = transform.position;
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
            if (!soundPlayed)
            {
                vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                soundPlayed = true;
            }
        }
    }

    public void CrossPattern(float offset = 0.3f)
    {
        GameObject vfx;
        List<GameObject> bullets = new List<GameObject>();

        bool soundPlayed = false;

        float[] yOffsets = { -2, -1, 0, 1, 2, 0, 0, 0, 0};
        float[] zOffsets = { 0, 0, 0, 0, 0, -2, -1, 1, 2};

        for (int jj = 0; jj < 9; jj++)
        {
            vfx = getBullet();
            if (vfx == null) continue;

            vfx.transform.position = transform.position;
            vfx.transform.rotation = Quaternion.identity;

            GameObject head = Camera.main.gameObject;
            Vector3 relativePos2Player = head.transform.position - transform.position;

            Quaternion relativeRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), relativePos2Player.normalized);
            Vector3 newY = relativeRotation * new Vector3(0, 1, 0);
            Vector3 newZ = relativeRotation * new Vector3(0, 0, 1);

            Vector3 dir = head.transform.position + newY * yOffsets[jj] * offset
                + newZ * zOffsets[jj] * offset - transform.position;
            vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
            if (!soundPlayed)
            {
                vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                soundPlayed = true;
            }
        }
    }

    public void DiePattern(float offset = 0.15f)
    {
        GameObject vfx;
        List<GameObject> bullets = new List<GameObject>();

        bool soundPlayed = false;

        float[] yOffsets = { -2, -2, -1, 0 , 1, 2, 2, 1, 0, -1,
            -2, -2, -2, -1, 0, 1, 2, 2, 2,
            -2, -2, -2, -1, 0, 0, 1, 2, 2, 2};
        float[] zOffsets = { -5, -4, -3, -3, -3, -4, -5, -5, -5, -5,
            -1, 0, 1, 0, 0, 0, -1, 0, 1,
            3, 4, 5, 3, 3, 4, 3, 3, 4, 5};

        for (int jj = 0; jj < 29; jj++)
        {
            vfx = getBullet();
            if (vfx == null) continue;

            vfx.transform.position = transform.position;
            vfx.transform.rotation = Quaternion.identity;

            GameObject head = Camera.main.gameObject;
            Vector3 relativePos2Player = head.transform.position - transform.position;

            Quaternion relativeRotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), relativePos2Player.normalized);
            Vector3 newY = relativeRotation * new Vector3(0, 1, 0);
            Vector3 newZ = relativeRotation * new Vector3(0, 0, 1);

            Vector3 dir = head.transform.position + newY * yOffsets[jj] * offset
                + newZ * zOffsets[jj] * offset - transform.position;
            vfx.GetComponent<ProjectileMoveScript2>().SetDirection(dir);
            if (!soundPlayed)
            {
                vfx.GetComponent<ProjectileMoveScript2>().playSpawnSound();
                soundPlayed = true;
            }
        }
    }
}
