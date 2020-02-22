using System;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class GazeGestureManager : MonoBehaviour
{
    public static GazeGestureManager Instance { get; private set; }

    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }

    GestureRecognizer recognizer;

    // Use this for initialization
    void Awake()
    {
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.DoubleTap);
        recognizer.Tapped += Recognizer_TappedEvent;
        recognizer.StartCapturingGestures();
    }

    private void Recognizer_TappedEvent(TappedEventArgs tappedEventArgs)
    {
        print("TappedEvent recognised!");
        if(tappedEventArgs.tapCount == 1)
        {
            // Do a raycast into the world based on the user's
            // head position and orientation.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;

            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                // Shoot projectile towards the point
                FiringProjectiles.FireBulletToPosition(hitInfo.point);
            }
        }
        else
        {
            FiringProjectiles.magazine.UnionWith(FiringProjectiles.shells);
            AlignAmmo.UpdateAmmoCount(FiringProjectiles.magazine.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;

        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        {
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }
    }
}