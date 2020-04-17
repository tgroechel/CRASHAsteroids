using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crash {
    public class WorldManager : Singleton<WorldManager> {
        public GameObject rootPoint, navMeshObject, enemyParent;

        private void Start() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            if (arg0.name == "Main") {
                StartCoroutine(LoadAfter3Seconds());
            }
        }


        IEnumerator LoadAfter3Seconds() {
            rootPoint.SetActive(false);
            yield return new WaitForSeconds(1);
            rootPoint.SetActive(true);
            yield return new WaitForSeconds(2);
            navMeshObject.SetActive(true);
            enemyParent.SetActive(true);
        }
    }
}
