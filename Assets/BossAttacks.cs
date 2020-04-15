using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{

    private BossAttackScript leftGun;
    private BossLaserScript laser;

    void Start()
    {
        leftGun = GameObject.Find("Barrel_end_1_right").GetComponent<BossAttackScript>();
        laser = GameObject.Find("Barrel_end_laser").GetComponent<BossLaserScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            leftGun.Pattern0();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            leftGun.Pattern1(Random.Range(0, 1000));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            leftGun.Pattern2(Random.Range(0, 1000));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            leftGun.Pattern3(Random.Range(0, 1000));
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            leftGun.CircularPattern();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            leftGun.CrossPattern();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            leftGun.DiePattern();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            laser.startLaser();
        }
    }

    // just a single boring bullet
    public void Pattern0()
    {
        leftGun.Pattern0();
    }

    // one bullet with some random offeset
    public void Pattern1()
    {
        leftGun.Pattern1(Random.Range(0, 1000));
    }

    // an array of bullets
    public void Pattern2()
    {
        leftGun.Pattern2(Random.Range(0, 1000));
    }

    // an array of bullets but slightly different
    public void Pattern3()
    {
        leftGun.Pattern3(Random.Range(0, 1000));
    }

    // a circle
    public void CircularPatter()
    {
        leftGun.CircularPattern();
    }

    // a cross
    public void CrossPatter()
    {
        leftGun.CrossPattern();
    }

    // die b**** !
    public void DiePatter()
    {
        leftGun.DiePattern();
    }

    // laser!
    public void Laser()
    {
        laser.startLaser();
    }
}
