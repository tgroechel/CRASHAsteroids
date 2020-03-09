using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRASHSceneUnderstandingUtil : MonoBehaviour
{
    /// <summary>
    /// Gives the position where robot could be instantiated two meters away from the player and stays on ground
    /// </summary>
    /// <returns>Feasible instantiate position of the robot</returns>
    public static Vector3 GetRobotInitialPosition()
    {
        Vector3 startPoint = Camera.main.transform.position + (Camera.main.transform.forward * 2);
        Vector3 direction = Camera.main.transform.up * -1;

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, Mathf.Infinity))
        {
            Debug.Log("Floor detected, use hit point of downard raycast 2 meters away from player as robot initial position");
            if (hit.collider.gameObject.name == "Floor")
            {
                return hit.point;
            }
        }

       Debug.Log("Floor not detected, failover to use point 2 meters away from player as robot initial position");
       return Camera.main.transform.position + (Camera.main.transform.forward * 2);
    }
}
