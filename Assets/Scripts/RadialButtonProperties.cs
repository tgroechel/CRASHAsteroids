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
    }
}
