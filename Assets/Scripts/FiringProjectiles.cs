using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringProjectiles : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityInputActionHandler, IMixedRealityGestureHandler
{
    public static GameObject bullet;
    public static float bulletSpeed = 2;
    public static string bulletName;
    public static Vector3 bulletRelativeToCamera = new Vector3(0, 0, 1f);
    public static float offsetX = 0, offsetY = 0.05f, offsetZ = 0;
    public static string firingHandHoloLens2 = "Mixed Reality Controller Right";
    public static string reloadingHandHoloLens2 = "Mixed Reality Controller Left";
    public static string firingHandUnity = "Right Hand";
    public static string reloadingHandUnity= "Left Hand";

    void Awake()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityGestureHandler>(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set default bullet name
        bulletName = ResourcePathManager.bullet1;

        // Load default bullet
        bullet = Resources.Load<GameObject>(ResourcePathManager.projectilesFolder + bulletName) as GameObject;
    }

    private void Update()
    {
        
    }

    // Fires a bullet (towards the position given) if present in the magazine, else prompts to reload
    public static void FireBulletToPosition(Vector3 targetPosition)
    {
        MixedRealityPose pose;
        Vector3 bulletSpawnPosition;

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, firingHandUnity == "Right Hand" || firingHandHoloLens2 == "Mixed Reality Controller Right" ? Handedness.Right : Handedness.Left, out pose))
        {
            bulletSpawnPosition = pose.Position
            + pose.Rotation * Vector3.forward * offsetZ
            + pose.Rotation * Vector3.up * offsetY
            + pose.Rotation * Vector3.right * offsetX;
        }
        else
            bulletSpawnPosition = Camera.main.transform.position + bulletRelativeToCamera;

        GameObject vfx = Instantiate(bullet, bulletSpawnPosition, Quaternion.identity);

        //vfx.GetComponent<ProjectileMoveScript>().SetTarget(GameObject.Find("BossBot"), null);
        vfx.GetComponent<ProjectileMoveScript>().SetTargetPos(targetPosition);
        vfx.GetComponent<ProjectileMoveScript>().speed = bulletSpeed;
    }

    // Fires a bullet (in the direction given) if present in the magazine, else prompts to reload
    public static void FireBulletInDirection(Quaternion targetDirection)
    {

    }

    public static void ChangeBullet(string newBulletName)
    {
        // Change name of bulletName to newBulletName
        bulletName = newBulletName;

        // Obtain the new bullet 
        bullet = Resources.Load<GameObject>(ResourcePathManager.projectilesFolder + newBulletName) as GameObject;
        
        print("Changed bullet to: " + newBulletName);
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
        if (eventData.InputSource.SourceName == firingHandHoloLens2 || eventData.InputSource.SourceName == firingHandUnity)
        {
            var result = eventData.Pointer.Result;
            var targetposition = result.Details.Point;
            FireBulletToPosition(targetposition);
        }

        // To check which source was used for pointer click
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
        
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
