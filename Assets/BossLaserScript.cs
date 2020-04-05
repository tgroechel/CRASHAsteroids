using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserScript : MonoBehaviour
{
    private GameObject effect;
    private GameObject laser;
    private EGA_Laser laserScript;
    private float timeElapsed;
    public AudioClip shotSFX;
    

    // Start is called before the first frame update
    void Start()
    {
        effect = Resources.Load<GameObject>(ResourcePathManager.bossLaser) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (laser != null)
        {
            Vector3 direction = GameObject.Find("PlayerHead").transform.position - laser.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            laser.transform.localRotation = Quaternion.Lerp(laser.transform.rotation, rotation, 1);
            RaycastHit hit;
            Physics.Raycast(laser.transform.position, laser.transform.TransformDirection(Vector3.forward), out hit, 30f);
            Debug.Log(hit.collider.gameObject.name);
            if (timeElapsed > 0.6f)
                stop();
        }
    }

    public void shoot()
    {
        if (shotSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
        Destroy(laser);
        laser = Instantiate(effect, transform.position, transform.rotation);
        laserScript = laser.GetComponent<EGA_Laser>();
        timeElapsed = 0;
    }

    public void stop()
    {
        laserScript.DisablePrepare();
        Destroy(laser, 1);
    }

}
