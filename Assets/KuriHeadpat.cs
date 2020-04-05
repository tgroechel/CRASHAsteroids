﻿using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class KuriHeadpat : MonoBehaviour, IMixedRealityTouchHandler {
        NearInteractionTouchable nearInteractionTouchable;

        public void OnTouchCompleted(HandTrackingInputEventData eventData) {

        }

        public void OnTouchStarted(HandTrackingInputEventData eventData) {
            KuriManager.instance.AlertHeadPat();
        }

        public void OnTouchUpdated(HandTrackingInputEventData eventData) {

        }

    }
}