using Microsoft.MixedReality.SceneUnderstanding.Samples.Unity;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaterTightDetector : MonoBehaviour{
    public GameObject SC;
    public GameObject root;
    public Text progressText;
    public Slider progressBar;
    public int ballCount = 100;
    public float threshold = 0.8f;
    public int checkInterval = 2;

    public CreateNavMeshesAndNavMeshLinks navObj;
    public Canvas loadCanvas;
    public Canvas hudCanvas;

    public bool isWaterTight = false;
    private bool detectionDone = false;
    private List<GameObject> spheres = new List<GameObject>();
    private DateTime lastLaunch;
    private static String detectionText = "Scanning environment ... ";
    private static String generatingText = "Scanning complete, Generating NavMesh";

    
    // Start is called before the first frame update
    void Start() {
        navObj = root.GetComponent<CreateNavMeshesAndNavMeshLinks>();
        hudCanvas.gameObject.SetActive(false);
        // Instanciate sphere objects, disable them and put them in object pool
        InstanciateSpheres(ballCount);
        LaunchSpheres();
    }

    void Update() {
        if (detectionDone)
        {
            progressBar.value = (float) navObj.percentageCompleted/100;
            if (!navObj.showLoading)
            {
                loadCanvas.gameObject.SetActive(false);
                hudCanvas.gameObject.SetActive(true);
            }
            return;
        }

        checkSpheres(GetSceneObjectsCenter());
        if (isWaterTight) {
            detectionDone = true;
            progressText.text = generatingText;
            progressBar.value = progressBar.maxValue;
            for (int i = 0; i < spheres.Count; i++)
            {
                Destroy(spheres[i]);
            }
            SC.GetComponentInChildren<SceneUnderstandingDataProvider>().gameObject.SetActive(false);
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
            MeshRenderer mr = spheres[i].GetComponent<MeshRenderer>();
            mr.enabled = false;
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

        float progress = (float)(ballCount - fallCount) / ballCount;
        progressText.text = detectionText + progress.ToString("P");
        progressBar.value = progress;

        if (progress < threshold) {
            isWaterTight = false;
            ResetSpheres(position);
            LaunchSpheres();
            return;
        }

        isWaterTight = true;
    }

}
