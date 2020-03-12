using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class CustomTwistPublisher : Publisher<Messages.Geometry.Twist> {

        private Messages.Geometry.Twist message;
        public Vector3 linearTwist;
        public Vector3 angularTwist;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                UpdateMessage();
            }
        }

        private void InitializeMessage() {
            message = new Messages.Geometry.Twist();
            message.linear = new Messages.Geometry.Vector3();
            message.angular = new Messages.Geometry.Vector3();
        }

        private void UpdateMessage() {
            message.linear = GetGeometryVector3(linearTwist.Unity2Ros()); ;
            message.angular = GetGeometryVector3(angularTwist.Unity2Ros());
            Publish(message);
            Debug.Log(message);
        }
        public void Forward() {
            PublishTwist(Vector3.forward, Vector3.zero);
        }
        public void Back() {
            PublishTwist(Vector3.back, Vector3.zero);
        }

        public void Right() {
            PublishTwist(Vector3.zero, new Vector3(0, -Mathf.PI / 2, 0));
        }

        public void Left() {
            PublishTwist(Vector3.zero, new Vector3(0, Mathf.PI / 2, 0));
        }

        public void PublishTwist(Vector3 lin, Vector3 ang) {
            linearTwist = lin;
            angularTwist = ang;
            UpdateMessage();
        }

        private static Messages.Geometry.Vector3 GetGeometryVector3(Vector3 vector3) {
            Messages.Geometry.Vector3 geometryVector3 = new Messages.Geometry.Vector3();
            geometryVector3.x = (float)vector3.x;
            geometryVector3.y = (float)vector3.y;
            geometryVector3.z = (float)vector3.z;
            return geometryVector3;
        }
    }
}
