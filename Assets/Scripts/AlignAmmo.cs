using UnityEngine;
using TMPro;
using Crash;

namespace Sid
{
    public class AlignAmmo : Singleton<AlignAmmo>
    {
        public static TextMeshProUGUI textComponent;
        public static GameObject cam;
        public float offsetX = -0.395f;
        public float offsetY = 0.170f;
        public float offsetZ = 0.310f;

        // Start is called before the first frame update
        void Awake()
        {
            cam = GameObject.Find("Main Camera");
            textComponent = GetComponent<TextMeshProUGUI>();
            textComponent.SetText("Ammo Remaining " + FiringProjectilesReuse.magazineSize);
        }

        // Update is called once per frame
        void Update()
        {
            var camRotation = Camera.main.transform.rotation;
            transform.position = cam.transform.position
                + cam.transform.rotation * Vector3.forward * offsetZ
                + cam.transform.rotation * Vector3.up * offsetY
                + cam.transform.rotation * Vector3.right * offsetX;
            transform.rotation = camRotation;
        }

        // Update ammo count
        public static void UpdateAmmoCount(int bulletsLeft)
        {
            if (bulletsLeft == FiringProjectilesReuse.magazineSize)
                textComponent.SetText("Ammo Remaining " + bulletsLeft + "\n" + "Reloaded!");
            else if (bulletsLeft > 0)
                textComponent.SetText("Ammo Remaining " + bulletsLeft);
            else
                textComponent.SetText("Ammo Remaining " + bulletsLeft + "\n" + "Reload!");
        }
    }
}