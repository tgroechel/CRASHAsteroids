using System.Collections;
using UnityEngine;

namespace Whatever {
    public class SnapCollider : MonoBehaviour {

        Rigidbody rb;
        ControllerComponent curControllerComponentInContact;

        private void Awake() {
            SnapColliderManager.instance.RegisterSnapCollider(this);
            GetComponent<Collider>().isTrigger = true;
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider collision) {
            ControllerComponent cc = collision.transform.GetComponent<ControllerComponent>();
            if (cc != null && cc == ControllerComponent.curDraggingControllerComponent) {
                curControllerComponentInContact = cc;
                curControllerComponentInContact.SetSnapColInContact(this);
            }
        }

        private void OnTriggerExit(Collider collision) {
            curControllerComponentInContact?.SetSnapColInContact(null);
            curControllerComponentInContact = null;
        }

        public void DoSnapAction() {
            if (curControllerComponentInContact != null) {
                curControllerComponentInContact.RemoveFromParentControllerComponent();
                curControllerComponentInContact.transform.SnapToParent(transform.parent, transform.localPosition);
                GetMyControllerComponent().AddComponentToAttached(curControllerComponentInContact);
            }
            curControllerComponentInContact = null;
        }

        public ControllerComponent GetMyControllerComponent() { // TODO: is hack fix later
            return transform.parent.parent.GetComponent<ControllerComponent>();
        }

        private void OnDestroy() {
            if (SnapColliderManager.instance && SnapColliderManager.instance.isActiveAndEnabled) {
                SnapColliderManager.instance?.DeregisterSnapCollider(this);
            }
        }

    }
}