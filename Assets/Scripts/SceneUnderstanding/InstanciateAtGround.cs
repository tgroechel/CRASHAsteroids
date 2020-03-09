using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateAtGround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = CRASHSceneUnderstandingUtil.GetRobotInitialPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
