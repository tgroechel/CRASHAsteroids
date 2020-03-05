using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tomato {
    public class MoveTheCube : MonoBehaviour {
        bool isMoving = false;
        public AnimationCurve animCurve;

        IEnumerator MoveMe() {
            if (isMoving) {
                yield break;
            }
            isMoving = true;

            Vector3 targetPosition = transform.localPosition + Vector3.up * 3;
            Vector3 origPosition = transform.localPosition;
            float timePassed = 0;
            float totalTime = 2f;
            while (timePassed < totalTime) {
                float percentDone = timePassed / totalTime;
                transform.localPosition = Vector3.Lerp(origPosition, targetPosition, animCurve.Evaluate(percentDone));
                yield return new WaitForSeconds(Time.deltaTime);
                timePassed += Time.deltaTime;
            }


            isMoving = false;
        }
        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                StartCoroutine(MoveMe());
                GetComponent<MeshRenderer>().material = Resources.Load<Material>(ResourcePathManager.JunkMaterialPath) as Material;
            }
        }
    }
}