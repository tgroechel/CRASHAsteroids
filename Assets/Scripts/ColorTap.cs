using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class ColorTap : MonoBehaviour, IMixedRealityFocusHandler
{
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public float lengthOfLineRenderer = 10.0f;

    private Color color_IdleState = Color.cyan;
    private Color color_OnHover = Color.white;
    private Color color_OnSelect = Color.blue;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;

        InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
        InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
        InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 2;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        
    }

    private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
    {
        print("Hand position detected!");
    }

    // Whenever the hand position is updated, a line ray should be emerging from the index finger tip
    private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
    {
        var interactionSourceState = args.state;
        var sourcePose = interactionSourceState.sourcePose;
        Vector3 sourceGripPosition;
        Quaternion sourceGripRotation;
        if ((sourcePose.TryGetPosition(out sourceGripPosition, InteractionSourceNode.Pointer)) &&
            (sourcePose.TryGetRotation(out sourceGripRotation, InteractionSourceNode.Pointer)))
        {
            //RaycastHit raycastHit;
            //if (Physics.Raycast(sourceGripPosition, sourceGripRotation * Vector3.forward, out raycastHit, 10))
            //{
            //    var targetObject = raycastHit.collider.gameObject;
            //    // ...
            //}

            //LineRenderer lineRenderer = GetComponent<LineRenderer>();
            //var points = new Vector3[2];
            //points[0] = sourceGripPosition;
            //points[1] = sourceGripPosition + sourceGripRotation * Vector3.forward * lengthOfLineRenderer;
            //lineRenderer.SetPositions(points);

        }
        print("Hand position updated!");
    }

    private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs obj)
    {
        print("Hand position lost!");
    }



    void IMixedRealityFocusHandler.OnFocusEnter(FocusEventData eventData)
    {
        material.color = color_OnHover;
    }

    void IMixedRealityFocusHandler.OnFocusExit(FocusEventData eventData)
    {
        material.color = color_IdleState;
    }    
}