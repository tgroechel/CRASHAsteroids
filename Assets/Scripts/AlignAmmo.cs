using UnityEngine;
using TMPro;

public class AlignAmmo : MonoBehaviour
{
    public static TextMeshProUGUI textComponent;
    public static Vector3 ammoRelativeToCamera = new Vector3(-1.50f, 0.6f, 1f);

    // Start is called before the first frame update
    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.SetText("Ammo Remaining " + FiringProjectiles.magazineSize);
    }

    // Update is called once per frame
    void Update()
    {
        var headPosition = Camera.main.transform.position + ammoRelativeToCamera;
        var gazeDirection = Camera.main.transform.forward;
        transform.position = headPosition;
        // transform.rotation = Quaternion.FromToRotation(Vector3.up, gazeDirection);
    }

    // Update ammo count
    public static void UpdateAmmoCount(int bulletsLeft)
    {
        if(bulletsLeft == FiringProjectiles.magazineSize)
            textComponent.SetText("Ammo Remaining " + bulletsLeft + "\n" + "Reloaded!");
        else if(bulletsLeft > 0)
            textComponent.SetText("Ammo Remaining " + bulletsLeft);
        else
            textComponent.SetText("Ammo Remaining " + bulletsLeft + "\n" + "Reload!");
    }
}
