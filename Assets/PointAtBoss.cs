using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtBoss : MonoBehaviour
{
    private GameObject boss;
    private GameObject cam;

    private float yAngle = 0;

    public float offsetX = 0;
    public float offsetY = 0;
    public float offsetZ = 0;


    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.Find("Cola Can");
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.transform.position
            + cam.transform.rotation * Vector3.forward * offsetZ
            + cam.transform.rotation * Vector3.up * offsetY
            + cam.transform.rotation* Vector3.right * offsetX;

        Vector3 relativePos2Boss = boss.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos2Boss, Vector3.up);
        rotation *= Quaternion.Euler(90, 0, 0) * Quaternion.Euler(0, yAngle += 5, 0);
        transform.rotation = rotation;

        //
    }
}
