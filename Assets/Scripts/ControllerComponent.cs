using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Whatever {
    public class ControllerComponent : MonoBehaviour {

        public static ControllerComponent curDraggingControllerComponent;
        ManipulationHandler manipulationHandler;
        List<SnapCollider> mySnapColliders;
        SnapCollider snapColInContact;
        HashSet<ControllerComponent> attachedComponents;

        private void Start() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(OnManipulationStart);
            manipulationHandler.OnManipulationEnded.AddListener(OnManipulationEnd);
        }

        public List<SnapCollider> GetMySnapColliders() {
            if (mySnapColliders == null) {
                mySnapColliders = GetComponentsInChildren<SnapCollider>().ToList();
            }
            return mySnapColliders;
        }

        public HashSet<ControllerComponent> GetAttachedControllerComponents() {
            if (attachedComponents == null) {
                attachedComponents = new HashSet<ControllerComponent>();
            }
            return attachedComponents;
        }

        public void AddComponentToAttached(ControllerComponent cc) {
            GetAttachedControllerComponents().Add(cc);
        }

        public void RemoveComponentAttached(ControllerComponent cc) {
            GetAttachedControllerComponents().Remove(cc);
        }

        private void OnManipulationStart(ManipulationEventData arg0) {
            curDraggingControllerComponent = this;
            SnapColliderManager.instance.EnableColliders();
            DisableMySnapColliders();
        }

        private void OnManipulationEnd(ManipulationEventData arg0) {
            curDraggingControllerComponent = null;
            if (snapColInContact == null) {
                RemoveFromParentControllerComponent();
                transform.SnapToSnapManager();
            }
            else {
                snapColInContact.DoSnapAction();
                snapColInContact = null;
            }
            SnapColliderManager.instance.DisableColliders();
        }


        public void RemoveFromParentControllerComponent() {
            ControllerComponent cc = GetParentControllerComponent();
            if (cc) {
                cc.RemoveComponentAttached(this);
            }
        }

        public ControllerComponent GetParentControllerComponent() {
            return transform.parent?.parent?.GetComponent<ControllerComponent>();
        }

        public void SetSnapColInContact(SnapCollider sc) {
            snapColInContact = sc;
        }

        void DisableMySnapColliders() {
            ToggleMySnapColliders(false);
        }

        void EnableMySnapColliders() {
            ToggleMySnapColliders(true);
        }

        void ToggleMySnapColliders(bool b) {
            foreach (SnapCollider sc in GetMySnapColliders()) {
                sc.gameObject.SetActive(b);
            }
            foreach (ControllerComponent cc in GetAttachedControllerComponents()) {
                cc.ToggleMySnapColliders(b);
            }
        }
    }
}
