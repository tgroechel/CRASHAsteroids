using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacks : MonoBehaviour
{

    private BossAttackScript leftGun;
    private BossAttackScript rightGun;
    private BossLaserScript laser;

    void Start()
    {
        leftGun = GameObject.Find("Barrel_end_1_left").GetComponent<BossAttackScript>();
        rightGun = GameObject.Find("Barrel_end_1_right").GetComponent<BossAttackScript>();
        laser = GameObject.Find("Barrel_end_laser").GetComponent<BossLaserScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Pattern0('l');
            Pattern0('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Pattern1('l');
            Pattern1('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Pattern2('l');
            Pattern2('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Pattern3('l');
            Pattern3('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CircularPattern('l');
            CircularPattern('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CrossPattern('l');
            CrossPattern('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            DiePattern('l');
            DiePattern('r');
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Laser();
        }
    }

    // just a single boring bullet
    public void Pattern0(char c)
    {
        if (c == 'l')
            leftGun.Pattern0();
        else
            rightGun.Pattern0();
    }

    // one bullet with some random offeset
    public void Pattern1(char c)
    {
        if (c == 'l')
            leftGun.Pattern1(Random.Range(0, 1000));
        else
            rightGun.Pattern1(Random.Range(0, 1000));
    }

    // an array of bullets
    public void Pattern2(char c)
    {
        if (c == 'l')
            leftGun.Pattern2(Random.Range(0, 1000));
        else
            rightGun.Pattern2(Random.Range(0, 1000));
    }

    // an array of bullets but slightly different
    public void Pattern3(char c)
    {
        if (c == 'l')
            leftGun.Pattern3(Random.Range(0, 1000));
        else
            rightGun.Pattern3(Random.Range(0, 1000));
    }

    // a circle
    public void CircularPattern(char c)
    {
        if (c == 'l')
            leftGun.CircularPattern();
        else
            rightGun.CircularPattern();
    }

    // a cross
    public void CrossPattern(char c)
    {
        if (c == 'l')
            leftGun.CrossPattern();
        else
            rightGun.CrossPattern();
    }

    // die b**** !
    public void DiePattern(char c)
    {
        if (c == 'l')
            leftGun.DiePattern();
        else
            rightGun.DiePattern();
    }

    // laser!
    public void Laser()
    {
        laser.startLaser();
    }
}
