﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{

    private Dictionary<string, List<GameObject>> pools;

    void Start()
    {
    }

    public void createPool(string name, GameObject target, int size)
    {
        if (pools == null)
            pools = new Dictionary<string, List<GameObject>>();
        pools.Add(name, new List<GameObject>());
        for (int i = 0; i < size; i++)
        {
            GameObject vfx = Instantiate(target, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            vfx.SetActive(false);
            pools[name].Add(vfx);
        }

        foreach (GameObject go in pools[name])
        {
            foreach (GameObject go2 in GameObject.FindGameObjectsWithTag("Boss"))
                if (go2.GetComponent<Collider>())
                    Physics.IgnoreCollision(go.GetComponent<Collider>(), go2.GetComponent<Collider>(), ignore: true);
            
            foreach (GameObject go2 in pools[name])
                Physics.IgnoreCollision(go.GetComponent<Collider>(), go2.GetComponent<Collider>(), ignore: true);
        }
    }

    public GameObject getInstance(string name)
    {
        for (int i = 0; i < pools[name].Count; i++)
        {

            if (!pools[name][i].activeInHierarchy)
            {
                pools[name][i].SetActive(true);
                return pools[name][i];
            }
        }
        return null;
    }

}
