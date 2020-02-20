// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Input;
using TMPro;
using UnityEngine;


public class CustomGestureHandler : MonoBehaviour, IMixedRealityGestureHandler
{
    public GameObject HoldIndicator = null;
    public GameObject ManipulationIndicator = null;
    public GameObject NavigationIndicator = null;
    public GameObject SelectIndicator = null;

    public Material DefaultMaterial = null;
    public Material HoldMaterial = null;
    public Material ManipulationMaterial = null;
    public Material NavigationMaterial = null;
    public Material SelectMaterial = null;

    public GameObject RailsAxisX = null;
    public GameObject RailsAxisY = null;
    public GameObject RailsAxisZ = null;

    public void OnGestureStarted(InputEventData eventData)
    {
        Debug.Log($"OnGestureStarted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
        }
        else if (action == "Manipulate Action")
        {
        }
        else if (action == "Navigation Action")
        {
        }
        print("gesture started!");

    }

    public void OnGestureUpdated(InputEventData eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
        }
        print("gesture updated!");
    }

    public void OnGestureUpdated(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Manipulate Action")
        {
        }
        else if (action == "Navigation Action")
        {
            
        }
        print("gesture updated!");
    }

    public void OnGestureCompleted(InputEventData eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
            print("Hold Action occurred on " + gameObject.name);
        }
        else if (action == "Select")
        {
            print("Select Action occurred on " + gameObject.name);
        }
        print("gesture completed!");
    }

    public void OnGestureCompleted(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Manipulate Action")
        {
        }
        else if (action == "Navigation Action")
        {
        }
        print("gesture completed!");
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        Debug.Log($"OnGestureCanceled [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        var action = eventData.MixedRealityInputAction.Description;
        if (action == "Hold Action")
        {
        }
        else if (action == "Manipulate Action")
        {
        }
        else if (action == "Navigation Action")
        {
        }
        print("gesture canceled!");
    }
}
