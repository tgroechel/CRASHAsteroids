using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringProjectilesReuse : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityInputActionHandler, IMixedRealityGestureHandler, IMixedRealityFocusHandler
{
    public static GameObject bullet;
    public static string bulletName;
    public static int magazineSize = 10;
    public static float velocity = 10;
    public static string firingHand = "Mixed Reality Controller Left";
    public static string reloadingHand = "Mixed Reality Controller Right";
    public static Vector3 bulletRelativeToCamera = new Vector3(0, 0, 1f);
    public static float offsetX = 0, offsetY = 0f, offsetZ = 0.5f;
    public static HashSet<GameObject> magazine;
    public static HashSet<GameObject> shells;


    void Awake()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityFocusHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityGestureHandler>(this);
        bulletName = ResourcePathManager.bullets[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        // Obtain the bullet prefab
        string pathToBullet = ResourcePathManager.projectilesFolder + bulletName;
        bullet = Resources.Load<GameObject>(pathToBullet) as GameObject;

        // Load the magazine with magazineSize number of bullets
        magazine = new HashSet<GameObject>();
        shells = new HashSet<GameObject>();
        for (int i = 0; i < magazineSize; i++)
        {
            GameObject newBullet = Instantiate(bullet, new Vector3(0, 0, 0), Quaternion.identity);
            newBullet.SetActive(false);
            magazine.Add(newBullet);
        }
    }

    private void Update()
    {

    }

    // Fires a bullet (towards the position given) if present in the magazine, else prompts to reload
    public static void FireBulletToPosition(Vector3 targetPosition)
    {
        if (magazine.Count > 0)
        {
            foreach (GameObject bullet in magazine)
            {
                magazine.Remove(bullet);
                shells.Add(bullet);
                bullet.SetActive(true);
                MixedRealityPose pose;
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, firingHand == "Right Hand" ? Handedness.Right : Handedness.Left, out pose))
                {
                    bullet.transform.position = pose.Position
                    + pose.Rotation * Vector3.forward * offsetZ
                    + pose.Rotation * Vector3.up * offsetY
                    + pose.Rotation * Vector3.right * offsetX;
                    print("I am in joints!");
                }
                else
                    bullet.transform.position = Camera.main.transform.position + bulletRelativeToCamera;
                bullet.transform.LookAt(targetPosition);
                bullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(targetPosition - bullet.transform.position) * velocity;
                print("Fired bullet!");
                AlignAmmo.UpdateAmmoCount(magazine.Count);
                break; // breaking here to ensure that only one bullet is fired per point click
            }
        }
        else
        {
            AlignAmmo.UpdateAmmoCount(magazine.Count);
            print("Magazine is empty! Please reload.");
        }

    }

    // Fires a bullet (in the direction given) if present in the magazine, else prompts to reload
    public static void FireBulletInDirection(Quaternion targetDirection)
    {
        if (magazine.Count > 0)
        {
            foreach (GameObject bullet in magazine)
            {
                magazine.Remove(bullet);
                shells.Add(bullet);
                bullet.SetActive(true);
                bullet.transform.position = Camera.main.transform.position + bulletRelativeToCamera;
                bullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(targetDirection * Vector3.forward) * velocity;
                print("Fired bullet!");
                AlignAmmo.UpdateAmmoCount(magazine.Count);
                break; // breaking here to ensure that only one bullet is fired per point click
            }
        }
        else
        {
            AlignAmmo.UpdateAmmoCount(magazine.Count);
            print("Magazine is empty! Please reload.");
        }

    }

    public static void ChangeBullet(string newBulletName)
    {
        if (newBulletName != bulletName)
        {
            // Clear magazine and shells
            magazine.Clear();
            shells.Clear();

            // Change name of bulletName to newBulletName
            bulletName = newBulletName;

            // Obtain the bullet prefab
            string pathToBullet = ResourcePathManager.projectilesFolder + bulletName;
            bullet = Resources.Load<GameObject>(pathToBullet) as GameObject;

            // Load the magazine with magazineSize number of bullets
            for (int i = 0; i < magazineSize; i++)
            {
                GameObject newBullet = Instantiate(bullet, new Vector3(0, 0, 0), Quaternion.identity);
                newBullet.SetActive(false);
                magazine.Add(newBullet);
                print("Added new bullet name: " + newBullet.name);
            }
        }
    }

    void IMixedRealityPointerHandler.OnPointerUp(
         MixedRealityPointerEventData eventData)
    { }

    void IMixedRealityPointerHandler.OnPointerDown(
         MixedRealityPointerEventData eventData)
    { }

    void IMixedRealityPointerHandler.OnPointerDragged(
         MixedRealityPointerEventData eventData)
    { }

    public void OnActionStarted(BaseInputEventData eventData)
    {
        //print("Action Started: " + eventData.MixedRealityInputAction.Description);
    }

    public void OnActionEnded(BaseInputEventData eventData)
    {
        //print("Action Ended " + eventData.MixedRealityInputAction.Description);

    }

    // Fire using firingHand and reload using reloadingHand
    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (eventData.InputSource.SourceName == firingHand)
        {
            // Get the coordinates of the point and fire a bullet towards that position
            var result = eventData.Pointer.Result;
            var targetposition = result.Details.Point;
            FireBulletToPosition(targetposition);
        }
        else if (eventData.InputSource.SourceName == reloadingHand)
        {
            magazine.UnionWith(shells);
            AlignAmmo.UpdateAmmoCount(magazine.Count);
        }

        // To check which gesture occurred and by which input source
        var currentText = AlignAmmo.textComponent.textInfo.textComponent.text;
        var actionDescription = eventData.MixedRealityInputAction.Description;
        var actionSource = eventData.InputSource.SourceName;
        var stringToAppend = "Pointer Action Completed: " + actionDescription + "\n" + "By Input Source: " + actionSource;
        AlignAmmo.textComponent.SetText(currentText + "\n\n" + stringToAppend);
    }

    public void OnGestureStarted(InputEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnGestureUpdated(InputEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnGestureCompleted(InputEventData eventData)
    {
        //if (eventData.InputSource.SourceName == firingHand)
        //{
        //    // Do a raycast into the world based on the user's
        //    // head position and orientation.
        //    var headPosition = Camera.main.transform.position;
        //    var gazeDirection = Camera.main.transform.forward;

        //    RaycastHit hitInfo;

        //    if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
        //    {
        //        // Shoot projectile towards the point
        //        FiringProjectiles.FireBulletToPosition(hitInfo.point);
        //    }
        //}
        //else
        //{
        //    magazine.UnionWith(shells);
        //    AlignAmmo.UpdateAmmoCount(magazine.Count);
        //}



        //// To check which gesture occurred and by which input source
        //var currentText = AlignAmmo.textComponent.textInfo.textComponent.text;
        //var actionDescription = eventData.MixedRealityInputAction.Description;
        //var actionSource = eventData.InputSource.SourceName;
        //var stringToAppend = "Gesture Completed: " + actionDescription + "\n" + "By Input Source: " + actionSource;
        //AlignAmmo.textComponent.SetText(currentText + "\n\n" + stringToAppend);
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnFocusEnter(FocusEventData eventData)
    {
        print("Focus Entered on: " + eventData.NewFocusedObject.name);
    }

    public void OnFocusExit(FocusEventData eventData)
    {
        print("Focus Exited from: " + eventData.OldFocusedObject.name);
    }
}
