using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class SnapColliderManager : Singleton<SnapColliderManager> {

        HashSet<SnapCollider> snapColliders;

        public HashSet<SnapCollider> GetAllSnapColliders() {
            if (snapColliders == null) {
                snapColliders = new HashSet<SnapCollider>();
            }
            return snapColliders;
        }

        public void RegisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Add(sIn);
        }

        public void DeregisterSnapCollider(SnapCollider sIn) {
            GetAllSnapColliders().Remove(sIn);
        }

        public void EnableColliders() {
            SetAllColliderState(true);
        }

        public void DisableColliders() {
            SetAllColliderState(false);
        }

        private void SetAllColliderState(bool desiredActiveState) {
            foreach (SnapCollider sc in GetAllSnapColliders()) {
                sc.gameObject.SetActive(desiredActiveState);
            }
        }
    }
}