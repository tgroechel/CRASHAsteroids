using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserScript : MonoBehaviour
{
    private GameObject effect;
    private GameObject aimLaser;
    private GameObject laser;
    private EGA_Laser laserScript;
    private float timeElapsed;

    private bool started;
    private bool laserShot;

    public AudioClip shotSFX;
    public AudioClip chargeSFX;
    

    // Start is called before the first frame update
    void Start()
    {
        effect = Resources.Load<GameObject>(ResourcePathManager.bossLaser) as GameObject;
        GameObject aimLaserEffect = Resources.Load<GameObject>(ResourcePathManager.aimLaser) as GameObject;
        aimLaser = Instantiate(aimLaserEffect, transform.position, transform.rotation);
        aimLaser.SetActive(false);
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (started)
        {
            if (timeElapsed < 5)
            {
                Vector3 direction = GameObject.Find("PlayerHead").transform.position - new Vector3(0,0,0.02f) - aimLaser.transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                aimLaser.transform.localRotation = Quaternion.Lerp(aimLaser.transform.rotation, rotation, 1);
            }
            else
            {
                if (!laserShot)
                {
                    shoot();
                }
                else {
                    RaycastHit hit;
                    Physics.Raycast(laser.transform.position, laser.transform.TransformDirection(Vector3.forward), out hit, 30f);
                    Debug.Log(hit.collider.gameObject.name);
                    if (timeElapsed > 7f)
                        stop();
                }
            }
        }
    }

    public void startLaser()
    {
        if (started)
            return;
        started = true;
        laserShot = false;
        aimLaser.SetActive(true);
        aimLaser.transform.position = transform.position + new Vector3(0,0,0.3f);
        timeElapsed = 0;
        if (chargeSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(chargeSFX);
        }
    }

    private void shoot()
    {
        aimLaser.SetActive(false);
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().Stop();
        }
        if (shotSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
        Destroy(laser);
        laser = Instantiate(effect, transform.position, transform.rotation);
        Vector3 direction = GameObject.Find("PlayerHead").transform.position - laser.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        laser.transform.localRotation = Quaternion.Lerp(laser.transform.rotation, rotation, 1);
        laserScript = laser.GetComponent<EGA_Laser>();
        laserShot = true;
    }

    public void stop()
    {
        laserScript.DisablePrepare();
        Destroy(laser, 1);
        started = false;
    }
}
