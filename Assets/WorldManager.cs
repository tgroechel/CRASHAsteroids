using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crash {
    public class WorldManager : Singleton<WorldManager> {
        GameObject rootPoint;
        GameObject navMeshObject;

        public static List<T> FindObjectsOfTypeAll<T>() {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(g => g.GetComponentsInChildren<T>(true))
                .ToList();
        }
        private void Awake() {
            rootPoint = FindObjectsOfTypeAll<CreateNavMeshesAndNavMeshLinks>()[0].gameObject;
            navMeshObject = FindObjectsOfTypeAll<NavMeshGameObjectParent>()[0].gameObject;
            if (!rootPoint.activeSelf) {
                rootPoint.SetActive(true);
            }
        }
        private void Start() {
            StartCoroutine(LoadAfter3Seconds());
        }

        IEnumerator LoadAfter3Seconds() {
            yield return new WaitForSeconds(3);
            navMeshObject.SetActive(true);
        }
    }
}
