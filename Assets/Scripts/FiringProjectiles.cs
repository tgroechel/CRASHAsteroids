using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringProjectiles : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityInputActionHandler
{
    public GameObject bullet;
    public static int magazineSize = 10;
    public static float velocity = 10;
    public static string firingHand = "Right Hand";
    public static string reloadingHand = "Left Hand";
    public static Vector3 bulletRelativeToCamera = new Vector3(0, 0, 0.5f);
    private static HashSet<GameObject> magazine;
    private static HashSet<GameObject> shells;

    
    void Awake()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Obtain the bullet prefab
        string pathToBullet = ResourcePathManager.projectilesFolder + ResourcePathManager.bullet1;
        bullet = Resources.Load<GameObject>(pathToBullet) as GameObject;

        // Load the magazine with magazineSize number of bullets
        magazine = new HashSet<GameObject>();
        shells = new HashSet<GameObject>();
        for(int i=0;i<magazineSize;i++)
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
    public static void FireBullet(Vector3 targetPosition)
    {
        if (magazine.Count > 0)
        {
            foreach(GameObject bullet in magazine)
            {
                magazine.Remove(bullet);
                shells.Add(bullet);
                bullet.SetActive(true);
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
                // bullet.transform.LookAt(targetPosition);
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
        print("Action Started: " + eventData.MixedRealityInputAction.Description);
    }

    public void OnActionEnded(BaseInputEventData eventData)
    {
        print("Action Ended " + eventData.MixedRealityInputAction.Description);
    }

    // Fire using firingHand and reload using reloadingHand
    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        print(eventData.InputSource.SourceName + " clicked!");
        if(eventData.InputSource.SourceName == firingHand)
        {
            var result = eventData.Pointer.Result;
            var targetPosition = result.Details.Point;
            // var targetRotation = Quaternion.LookRotation(result.Details.Normal);
            // FiringProjectiles.FireBullet(transform.position);
            FireBullet(targetPosition);
        }
        else if(eventData.InputSource.SourceName == reloadingHand)
        {
            magazine.UnionWith(shells);
            AlignAmmo.UpdateAmmoCount(magazine.Count);
        }
        print("Pointer Clicked!");
    }
}
