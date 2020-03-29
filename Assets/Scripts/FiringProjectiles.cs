using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crash;

namespace Sid
{
    public class FiringProjectiles : Singleton<FiringProjectiles>, IMixedRealityPointerHandler
    {
        public static GameObject bullet;
        public static float bulletSpeed = 2;
        public static string bulletName;
        public static Vector3 bulletRelativeToCamera = new Vector3(0, 0, 1f);
        public static float offsetX = 0, offsetY = 0.05f, offsetZ = 0;
        public static string firingHandHoloLens2 = "Mixed Reality Controller Right";
        public static string reloadingHandHoloLens2 = "Mixed Reality Controller Left";
        public static string firingHandUnity = "Right Hand";
        public static string reloadingHandUnity = "Left Hand";

        void Awake()
        {
            CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set default bullet name
            bulletName = ResourcePathManager.bullet1;

            // Load default bullet
            bullet = Resources.Load<GameObject>(ResourcePathManager.projectilesFolder + bulletName) as GameObject;
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

            Camera.main.GetComponent<CameraShakeSimpleScript>().ShakeCamera();
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
        {
            // Requirement for implementing the interface
        }

        void IMixedRealityPointerHandler.OnPointerDown(
             MixedRealityPointerEventData eventData)
        {
            // Requirement for implementing the interface
        }

        void IMixedRealityPointerHandler.OnPointerDragged(
             MixedRealityPointerEventData eventData)
        {
            // Requirement for implementing the interface
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
            //AlignAmmo.textComponent.SetText(currentText + "\n\n" + stringToAppend);
            AlignAmmo.textComponent.SetText(stringToAppend);
        }
    }
}