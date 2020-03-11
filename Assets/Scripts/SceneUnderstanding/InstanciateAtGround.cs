using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateAtGround : MonoBehaviour
{
    public float objectHeight = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Instanciate robot 2 meters away in front of player
        transform.position = CRASHSceneUnderstandingUtil.GetRobotInitialPosition(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Do anything with this game object's transfrom according to game logic here....

        // Update transform of this game object to make it stay on ground if any floor plane is detected
        Tuple<bool, Vector3> result = CRASHSceneUnderstandingUtil.GetFloorProjectionPoint(gameObject);
        
        // If any floor plane is deteced below this game object
        if (result.Item1) 
        {
            // Get projection point on floor plane and add height of this game object to obtain
            // the position that makes this game object appear as staying on ground
            Vector3 floorPos = result.Item2;
            floorPos.y = floorPos.y + objectHeight;
            
            // Move this game object to calculated position
            transform.position = floorPos;
        }
        
    }
}
