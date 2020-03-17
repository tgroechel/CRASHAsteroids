using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LoadRadialMenuButtons : MonoBehaviour
{
    public bool showRadialMenu = true;
    public static string selectedBulletName;
    // Creating a dictionary to store bullet references
    public static Dictionary<string, GameObject> radialMenuButtons = new Dictionary<string, GameObject>();
    public static Material bulletHighlightMaterial, bulletDeHighlightMaterial;
    public GameObject radialMenuButton;
    public GridObjectCollection goc;  
    public float offsetX = 0;
    public float offsetY = 0.04f;       
    public float offsetZ = -0.03f;   // Axis is along the lower arm
    public float xRot = -90;         // Axis is perpendicular to fingers and along the palm
    public float yRot = 0;
    public float zRot = 0;
    public float xRotNew = 90.0f;
    public float yRotNew = 0;
    public float zRotNew = 0;
    public float rotateMenu = 0;
    public static TextMeshProUGUI textComponent;
    MixedRealityPose poseThumbTip, poseIndexTip, poseMiddleTip, poseRingTip, posePinkyTip, poseWrist;

    //Start is called before the first frame update
    void Start()
    {
        // Obtain the material for highlighting selected bullet
        string pathToBulletHighlightMaterial = ResourcePathManager.materialsFolder + ResourcePathManager.bulletHighlightMaterial;
        bulletHighlightMaterial = Resources.Load<Material>(pathToBulletHighlightMaterial) as Material;

        // Obtain the material for delighlighting previously selected bullet
        string pathToBulletDeHighlightMaterial = ResourcePathManager.materialsFolder + ResourcePathManager.bulletDeHighlightMaterial;
        bulletDeHighlightMaterial = Resources.Load<Material>(pathToBulletDeHighlightMaterial) as Material;

        // Obtain the RadialMenuButton prefab
        string pathToButton = ResourcePathManager.prefabsFolder + ResourcePathManager.radialMenuButton;
        radialMenuButton = Resources.Load<GameObject>(pathToButton) as GameObject;

        // Obtain the GridObjectCollection script of the current object 
        goc = gameObject.GetComponent<GridObjectCollection>();

        // Load all projectiles into radial menu
        for (int i=0; i<ResourcePathManager.bullets.Count; i++)
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
            

        }
    }

    // Function to be called when a new weapon is obtained by player
    // newBullet is the prefab of the new weapon
    void UpdateRadialMenu(string newBulletName)
    {
        // Get a thumbnail of the new weapon for its radial menu button
        Texture2D bulletPreview = LoadTextureForBulletButton(newBulletName);

        // Create a new RadialMenuButton for the new weapon
        // and make it a child of the current object
        GameObject newButton = Instantiate(radialMenuButton, new Vector3(0, 0, 0), Quaternion.identity);
        newButton.transform.parent = gameObject.transform;
        newButton.GetComponent<RadialButtonProperties>().bulletName = newBulletName;
        print("Added new radial button");

        // Add the new button to the dictionary of all buttons
        radialMenuButtons.Add(newBulletName, newButton);

        // Add the obtained preview to the new button if it is not null
        if(bulletPreview != null)
        {
            //newButton.GetComponent<Renderer>().material.mainTexture = bulletPreview;
            //print("Added" + newBulletName + "thumbnail!");

            newButton.transform.Rotate(xRotNew, yRotNew, zRotNew, Space.Self);
            newButton.transform.GetChild(4).gameObject.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.mainTexture = bulletPreview;
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
