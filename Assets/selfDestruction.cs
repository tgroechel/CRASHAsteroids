using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfDestruction : MonoBehaviour
{
    private bool alive;

    void Start()
    {
        alive = true;   
    }

    void Update()
    {
        if (alive)
        {
            Destroy(gameObject, 0.5f);
            alive = false;
        }
    }
}
