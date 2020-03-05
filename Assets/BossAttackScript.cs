using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackScript : MonoBehaviour
{
    public float waitDuration = 1f;
    void Start()
    {
        StartCoroutine("ShootPlayer");
    }

    void Update()
    {
        GameObject head = GameObject.Find("PlayerHead");
        Vector3 relativePos2Player = head.transform.position - transform.position;
        relativePos2Player.y = 0;
        Quaternion rotation = Quaternion.LookRotation(relativePos2Player, Vector3.up);

        //rotation *= Quaternion.Euler(0, 0, 0) * Quaternion.Euler(0, 0, 0);
        transform.rotation = rotation;
    }

    IEnumerator ShootPlayer()
    {
        while (true)
        {
            GameObject effect = Resources.Load<GameObject>(ResourcePathManager.projectilesFolder + ResourcePathManager.bullet1) as GameObject;
            GameObject vfx = Instantiate(effect, transform.position, Quaternion.identity);
            vfx.GetComponent<ProjectileMoveScript>().speed = 3;
            GameObject head = GameObject.Find("PlayerHead");
            Vector3 targetPos = head.transform.position;
            vfx.GetComponent<ProjectileMoveScript>().SetTargetPos(targetPos);

            yield return new WaitForSeconds(waitDuration);
        }
    }
}
