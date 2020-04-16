using Microsoft.MixedReality.SceneUnderstanding.Samples.Unity;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaterTightDetector : MonoBehaviour, IMixedRealityPointerHandler {
    public GameObject SC;
    public GameObject root;
    public Text progressText;
    public int ballCount = 100;
    public float threshold = 0.8f;
    public int checkInterval = 2;
    public String nextScene;

    private bool isWaterTight = false;
    private List<GameObject> spheres = new List<GameObject>();
    private DateTime lastLaunch;
    private static String detectionText = "Scanning environment ... ";
    private static String completeText = "Scanning complete, tap to start game";

    public static string firingHandHoloLens2 = "Mixed Reality Controller Right";
    public static string firingHandUnity = "Right Hand";

    void Awake() {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityPointerHandler>(this);
    }

    // Start is called before the first frame update
    void Start() {
        // Instanciate sphere objects, disable them and put them in object pool
        InstanciateSpheres(ballCount);
        LaunchSpheres();
    }

    void Update() {
        checkSpheres(GetSceneObjectsCenter());
        if (isWaterTight) {
            progressText.text = completeText;
        }
    }

    public bool IsWaterTight() {
        return isWaterTight;
    }

    private Vector3 GetSceneObjectsCenter() {
        if (root.transform.childCount == 0) {
            return Camera.main.transform.position;
        }

        Vector3 position = Vector3.zero;
        for (int i = 0; i < root.transform.childCount; i++) {
            position += root.transform.GetChild(i).transform.position;
        }
        return position /= root.transform.childCount;
    }

    private void InstanciateSpheres(int amount) {
        for (int i = 0; i < amount; i++) {
            float zOffset = 5f;
            float x = (float)Math.Cos(Math.PI * 2 / amount * i);
            float z = (float)Math.Sin(Math.PI * 2 / amount * i);

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = gameObject.transform;
            sphere.transform.position = new Vector3(x, -10, z + zOffset);
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Rigidbody rb = sphere.AddComponent<Rigidbody>();
            rb.useGravity = true;
            sphere.SetActive(false);
            spheres.Add(sphere);
        }
    }

    private void LaunchSpheres() {
        lastLaunch = DateTime.Now;
        for (int i = 0; i < spheres.Count; i++) {
            spheres[i].SetActive(true);
            Rigidbody rb = spheres[i].GetComponent<Rigidbody>();

            float force = 300f;
            float x = (float)Math.Cos(Math.PI * 2 / spheres.Count * i);
            float z = (float)Math.Sin(Math.PI * 2 / spheres.Count * i);

            rb.AddForce(new Vector3(x * force, 0, z * force));
        }
    }

    private void ResetSpheres(Vector3 position) {
        for (int i = 0; i < spheres.Count; i++) {
            Rigidbody rb = spheres[i].GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            float x = (float)Math.Cos(Math.PI * 2 / spheres.Count * i);
            float z = (float)Math.Sin(Math.PI * 2 / spheres.Count * i);
            spheres[i].transform.position = new Vector3(position.x + x, position.y, position.z + z);
            spheres[i].SetActive(false);
        }
    }

    private void checkSpheres(Vector3 position) {
        if ((DateTime.Now - lastLaunch).Seconds < checkInterval) {
            return;
        }

        int fallCount = 0;

        for (int i = 0; i < spheres.Count; i++) {
            if (spheres[i].transform.position.y <= -3) {
                fallCount += 1;
            }
        }

        if ((float)(ballCount - fallCount) / ballCount < threshold) {
            progressText.text = detectionText + ((float)(ballCount - fallCount) / ballCount).ToString("P");
            isWaterTight = false;
            ResetSpheres(position);
            LaunchSpheres();
            return;
        }

        isWaterTight = true;
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) {
        //throw new NotImplementedException();
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) {
        //throw new NotImplementedException();
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData) {
        //throw new NotImplementedException();
    }

    public void OnPointerClicked(MixedRealityPointerEventData eventData) {
        //throw new NotImplementedException();
    }

    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData) {
        if (isWaterTight) {
            Debug.Log("Switch to next scene");
            SC.GetComponentInChildren<SceneUnderstandingDataProvider>().gameObject.SetActive(false);
            DontDestroyOnLoad(SC);
            SceneManager.LoadScene(nextScene);
        }
    }
}
