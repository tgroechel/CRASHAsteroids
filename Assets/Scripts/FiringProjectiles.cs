using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringProjectiles : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityInputActionHandler, IMixedRealityGestureHandler
{
    public GameObject bullet;

    // projectile related assets
    public List<GameObject> VFXs = new List<GameObject>();



    public static int magazineSize = 10;
    public static float velocity = 10;
    public static string firingHand = "Right Hand";
    public static string reloadingHand = "Left Hand";
    public static Vector3 bulletRelativeToCamera = new Vector3(0, 0, 1f);
    public static HashSet<GameObject> magazine;
    public static HashSet<GameObject> shells;

    public static GameObject spawnEffect;
    


    void Awake()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityGestureHandler>(this);
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
            //GameObject newBullet = Instantiate(, new Vector3(0, 0, 0), Quaternion.identity);
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
        GameObject effect = Resources.Load<GameObject>(ResourcePathManager.projectile) as GameObject;
        GameObject vfx = Instantiate(effect, Camera.main.transform.position, Quaternion.identity);
        //vfx.GetComponent<ProjectileMoveScript>().SetTarget(GameObject.Find("BossBot"), null);
        vfx.GetComponent<ProjectileMoveScript>().SetTargetPos(targetPosition);
        if (magazine.Count > 0)
        {
            foreach(GameObject bullet in magazine)
            {
                //GameObject effect = Resources.Load<GameObject>(ResourcePathManager.spawn) as GameObject;
                //GameObject vfx = Instantiate(effect, bullet.transform.position, Quaternion.identity);
                //bullet.GetComponent<ProjectileMoveScript>().SetTarget(targetPosition, rotateToMouse);
                //magazine.Remove(bullet);
                //shells.Add(bullet);
                //bullet.SetActive(true);
                //bullet.GetComponent<ProjectileMoveScript>().SetTargetPos(targetPosition);
                //bullet.transform.position = Camera.main.transform.position;// + bulletRelativeToCamera;
                //bullet.transform.LookAt(targetPosition);
                //bullet.GetComponent<Rigidbody>().velocity = Vector3.Normalize(targetPosition - bullet.transform.position) * velocity;
                //print("Fired bullet!");
                //AlignAmmo.UpdateAmmoCount(magazine.Count);
                //break; // breaking here to ensure that only one bullet is fired per point click
            }
        }
        else
        {
           // AlignAmmo.UpdateAmmoCount(magazine.Count);
            //print("Magazine is empty! Please reload.");
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
        //print("Action Started: " + eventData.MixedRealityInputAction.Description);
    }

    public void OnActionEnded(BaseInputEventData eventData)
    {
        //print("Action Ended " + eventData.MixedRealityInputAction.Description);
    }

    // Fire using firingHand and reload using reloadingHand
    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        //print(eventData.InputSource.SourceName + " clicked! " + eventData.MixedRealityInputAction.Description + "action detected!");
        //if (eventData.InputSource.SourceName == firingHand)
        //{
        //    var result = eventData.Pointer.Result;
        //    var targetposition = result.Details.Point;
        //    // var targetrotation = quaternion.lookrotation(result.details.normal); 
        //    // firingprojectiles.firebullet(transform.position);
        //    FireBulletToPosition(targetposition);
        //}
        //else if (eventData.InputSource.SourceName == reloadingHand)
        //{
        //    magazine.UnionWith(shells);
        //    AlignAmmo.UpdateAmmoCount(magazine.Count);
        //}
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
        if (eventData.InputSource.SourceName == firingHand)
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
            magazine.UnionWith(shells);
            AlignAmmo.UpdateAmmoCount(magazine.Count);
        }

        // To check which gesture occurred and by which input source
        var currentText = AlignAmmo.textComponent.textInfo.textComponent.text;
        var actionDescription = eventData.MixedRealityInputAction.Description;
        var actionSource = eventData.InputSource.SourceName;
        var stringToAppend = "Gesture Completed: " + actionDescription + "\n" + "By Input Source: " + actionSource;
        AlignAmmo.textComponent.SetText(currentText + "\n\n" + stringToAppend);
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
