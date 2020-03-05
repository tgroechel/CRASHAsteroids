/*
© Siemens AG, 2018
Author: Berkay Alp Cakal (berkay_alp.cakal.ct@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.SensorVisualization {
    public class LaserScanVisualizerSpheres : LaserScanVisualizer {
        [Range(0.01f, 0.1f)]
        public float objectWidth;
        public Material material;

        private GameObject[] LaserScan;
        private bool IsCreated = false;

        private void Create(int numOfSpheres) {
            LaserScan = new GameObject[numOfSpheres];

            for (int i = 0; i < numOfSpheres; i++) {
                LaserScan[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                DestroyImmediate(LaserScan[i].GetComponent<Collider>());
                LaserScan[i].name = "LaserScanSpheres";
                LaserScan[i].transform.localScale = objectWidth * Vector3.one;
                LaserScan[i].transform.SetParent(transform);

                // LaserScan[i].transform.localScale = 
                LaserScan[i].transform.localPosition = origin;
                LaserScan[i].GetComponent<Renderer>().material = material;
            }
            IsCreated = true;
        }

        protected override void Visualize() {
            if (!IsCreated)
                Create(directions.Length);
            Vector3 scaler = transform.localScale;
            for (int j = 0; j < directions.Length; j++) {
                int i = directions.Length - j - 1;
                LaserScan[i].SetActive(ranges[j] != 0);
                LaserScan[i].GetComponent<Renderer>().material.color = GetColor(ranges[j]);


                Vector3 posEnd = ranges[j] * directions[j];
                if (float.IsInfinity(posEnd.x) || float.IsNaN(posEnd.x)) {
                    posEnd = origin;
                }
                else if (posEnd.magnitude < range_min) {
                    posEnd = origin;
                }

                posEnd.y = 0;

                LaserScan[i].transform.localPosition = posEnd / scaler.x;
            }
        }

        protected override void DestroyObjects() {
            for (int i = 0; i < LaserScan.Length; i++)
                Destroy(LaserScan[i]);
            IsCreated = false;
        }

    }
}