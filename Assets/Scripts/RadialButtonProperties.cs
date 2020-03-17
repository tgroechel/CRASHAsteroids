using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialButtonProperties : MonoBehaviour
{
    public string bulletName; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Change the current weapon selection on click
    public void ChangeCurrentWeapon()
    {
        // To check that interactable was clicked
        print("Clicked Interactable and chose: " + bulletName);
        FiringProjectiles.ChangeBullet(bulletName);

        // Remove highlight from previously selected button if it was different
        // and highlight the newly selected button
        if (bulletName != LoadRadialMenuButtons.selectedBulletName)
        {
            if(LoadRadialMenuButtons.selectedBulletName != null)
            {
                LoadRadialMenuButtons.radialMenuButtons[LoadRadialMenuButtons.selectedBulletName].transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = LoadRadialMenuButtons.bulletDeHighlightMaterial;
            }
            LoadRadialMenuButtons.radialMenuButtons[bulletName].transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = LoadRadialMenuButtons.bulletHighlightMaterial;
            LoadRadialMenuButtons.selectedBulletName = bulletName;
        }        
    }
}
