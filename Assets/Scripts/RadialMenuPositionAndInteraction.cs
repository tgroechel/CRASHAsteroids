using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
// using UnityEditor;
using UnityEngine;
using TMPro;

namespace Sid
{
    public class RadialMenuPositionAndInteraction : MonoBehaviour
    {
        public bool showRadialMenu = true;
        public GameObject radialMenuButton;
        public GridObjectCollection goc;
        public float offsetX = 0;
        public float offsetY = 0.04f;
        public float offsetZ = -0.03f;   // Axis is along the lower arm
        public float xRot = -90;         // Axis is perpendicular to fingers and along the palm
        public float yRot = 0;
        public float zRot = 0;
        public float rotateMenu = 0;
        public static TextMeshProUGUI textComponent;
        MixedRealityPose poseThumbTip;
        MixedRealityPose poseIndexTip;
        MixedRealityPose poseMiddleTip;
        MixedRealityPose poseRingTip;
        MixedRealityPose posePinkyTip;
        MixedRealityPose poseWrist;

        //void Awake()
        //{
        //    print("I am here ------------------------------");
        //    // Creating a thumbnail for each projectile
        //    for (int i = 0; i < ResourcePathManager.bullets.Count; i++)
        //    {
        //        // Obtain the current bullet prefab
        //        string pathToBullet = ResourcePathManager.projectilesFolder + ResourcePathManager.bullets[i];
        //        GameObject currBullet = Resources.Load<GameObject>(pathToBullet) as GameObject;

        //        // Get a thumbnail of the current bullet for its radial menu button
        //        Texture2D bulletPreview = AssetPreview.GetAssetPreview(currBullet);

        //        // Save the thumbnail with the same name as that of the bullet in "Resources/Projectile Thumbails" folder
        //        byte[] bytes = bulletPreview.EncodeToPNG();
        //        File.WriteAllBytes("Assets/Resources/Projectile Thumbnails/" + currBullet.name + ".png", bytes);
        //        print("Saved " + gameObject.name + " thumbnail!");
        //    }
        //}

        //Start is called before the first frame update
        void Start()
        {
            // Obtain the GUI component for displaying joint poses


            // Obtain the RadialMenuButton prefab
            string pathToButton = ResourcePathManager.prefabsFolder + ResourcePathManager.radialMenuButton;
            radialMenuButton = Resources.Load<GameObject>(pathToButton) as GameObject;

            // Obtain the GridObjectCollection script of the current object 
            goc = gameObject.GetComponent<GridObjectCollection>();

            // Load all projectiles into radial menu
            for (int i = 0; i < ResourcePathManager.bullets.Count; i++)
            {
                // Obtain the current bullet's name
                string nameCurrBullet = ResourcePathManager.bullets[i];

                // Update the radial menu accordingly
                UpdateRadialMenu(nameCurrBullet);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (showRadialMenu)
            {
                if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out poseWrist))
                {
                    gameObject.transform.position = poseWrist.Position
                        + poseWrist.Rotation * Vector3.forward * offsetZ
                        + poseWrist.Rotation * Vector3.up * offsetY
                        + poseWrist.Rotation * Vector3.right * offsetX;
                    gameObject.transform.rotation = poseWrist.Rotation * Quaternion.Euler(xRot, yRot, zRot);
                }

                HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbTip, Handedness.Right, out poseThumbTip);
                HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out poseIndexTip);
                HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleTip, Handedness.Right, out poseMiddleTip);
                HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Right, out poseRingTip);
                HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Right, out posePinkyTip);

                // Showing hand joint positions in UI
                ShowJointPosition.textComponent.SetText("");
                string stringToBeShown = "Right Thumb Pos: " + (poseThumbTip.Position - poseWrist.Position) + "\n\n";
                stringToBeShown = stringToBeShown + "Right Index Pos: " + (poseIndexTip.Position - poseWrist.Position) + "\n\n";
                stringToBeShown = stringToBeShown + "Right Middle Pos: " + (poseMiddleTip.Position - poseWrist.Position) + "\n\n";
                stringToBeShown = stringToBeShown + "Right Ring Pos: " + (poseRingTip.Position - poseWrist.Position) + "\n\n";
                stringToBeShown = stringToBeShown + "Right Pinky Pos: " + (posePinkyTip.Position - poseWrist.Position) + "\n\n";
                ShowJointPosition.textComponent.SetText(stringToBeShown);

            }
        }

        // Function to be called when a new weapon is obtained by player
        // newBullet is the prefab of the new weapon
        void UpdateRadialMenu(string newBulletName)
        {
            // Get a thumbnail of the new weapon for its radial menu button
            Texture2D bulletPreview = LoadTextureForBulletButton(newBulletName);

            // Create a new interactable RadialMenuButton for the new weapon
            // and make it a child of the current object
            GameObject newButton = Instantiate(radialMenuButton, new Vector3(0, 0, 0), Quaternion.identity);
            newButton.transform.parent = gameObject.transform;
            newButton.GetComponent<RadialButtonProperties>().bulletName = newBulletName;
            print("Added new radial button");

            // Add the obtained preview to the new button if it is not null
            if (bulletPreview != null)
            {
                newButton.GetComponent<Renderer>().material.mainTexture = bulletPreview;
                print("Added" + newBulletName + "thumbnail!");
            }

            // Update the GridObjectCollection
            goc.UpdateCollection();

        }

        Texture2D LoadTextureForBulletButton(string bulletName)
        {
            // Obtain the current bullet thumbnail
            string pathToBulletThumbnail = ResourcePathManager.projectilesThumbnailsFolder + bulletName;
            Texture2D tex = Resources.Load<Texture2D>(pathToBulletThumbnail) as Texture2D;

            return tex;
        }
    }
}