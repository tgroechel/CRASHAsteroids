﻿/*
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

public abstract class LaserScanVisualizer : MonoBehaviour {
    protected Vector3 origin;
    protected Vector3[] directions;
    protected float range_min;
    protected float range_max;
    protected float[] ranges;

    protected bool IsNewSensorDataReceived;
    protected bool IsVisualized = false;

    abstract protected void Visualize();
    abstract protected void DestroyObjects();

    protected void Update() {
        if (!IsNewSensorDataReceived)
            return;

        IsNewSensorDataReceived = false;
        Visualize();
    }

    protected void OnDisable() {
        DestroyObjects();
    }

    public void SetSensorData(Vector3 _origin, Vector3[] _directions, float[] _ranges, float _range_min, float _range_max) {
        origin = _origin;
        directions = _directions;
        ranges = _ranges;
        range_min = _range_min;
        range_max = _range_max;
        IsNewSensorDataReceived = true;
    }

    protected Color GetColor(float distance) {
        return Color.HSVToRGB(distance / range_max, 1, 1);
    }

}