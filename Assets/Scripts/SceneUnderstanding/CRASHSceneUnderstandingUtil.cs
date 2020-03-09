using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRASHSceneUnderstandingUtil : MonoBehaviour
{
    [Tooltip("Prints debug message of this util on console or not.")]
    public static bool ShowDebugMessage = false;

    /// <summary>
    /// Gives the position where robot could be instantiated two meters away from the player and stays on ground
    /// </summary>
    /// <returns>
    /// Feasible instantiate position of the robot. 
    /// If no floor is deteced, returned position would be 2 meters directly in front of player's camera
    /// </returns>
    public static Vector3 GetRobotInitialPosition(GameObject robot)
    {
        Vector3 startPoint = Camera.main.transform.position + (Camera.main.transform.forward * 2);
        Vector3 direction = Camera.main.transform.up * -1;

        Tuple<bool, Vector3> result = GetFloorProjectionPoint(startPoint, direction);
        if (result.Item1)
        {
            if(ShowDebugMessage) Debug.Log("Floor detected, use hit point of downard raycast 2 meters away from player as robot initial position");
            return result.Item2;
        }
        else
        {
            if (ShowDebugMessage) Debug.Log("Floor not detected, failover to use point 2 meters away from player as robot initial position");
            return Camera.main.transform.position + (Camera.main.transform.forward * 2);
        }
    }

    /// <summary>
    /// Returns whether a floor plane is detected below the given object and it's projection point on the floor plane.
    /// </summary>
    /// <returns>
    /// A tuple. First item is a bool that indicates whether a floor plane is found.
    /// Second item is a Vector3 that returns the projection point on floor. If no floor is found, this item would be (0,0,0)
    /// </returns>
    public static Tuple<bool, Vector3> GetFloorProjectionPoint(GameObject go)
    {
        Vector3 startPoint = go.transform.position;
        Vector3 direction = go.transform.up * -1;

        return GetFloorProjectionPoint(startPoint, direction);
    }

    /// <summary>
    /// Returns whether a floor plane is detected by the raycast starting from "startPoint" to "direction" and raycast hit point
    /// </summary>
    /// <returns>
    /// A tuple. First item is a bool that indicates whether a floor plane is detected.
    /// Second item is a Vector3 that returns the hit point on floor. If no floor is found, this item would be (0,0,0)
    /// </returns>
    public static Tuple<bool, Vector3> GetFloorProjectionPoint(Vector3 startPoint, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.name == "Floor")
            {
                if(ShowDebugMessage) Debug.Log("Floor detected");
                return Tuple.Create(true, hit.point);
            }
        }

        if (ShowDebugMessage) Debug.Log("Floor not detected, return object's original position");
        return Tuple.Create(false, Vector3.zero);
    }
}
