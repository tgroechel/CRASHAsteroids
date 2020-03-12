using System.Collections;
using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class ChestLedPublisher : Publisher<Messages.Mayfield.ChestLeds> {

        private Messages.Mayfield.ChestLeds message;
        Messages.Mayfield.Led[] leds;
        public Color pubColor;


        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                StartCoroutine(ColorCycle());
            }
        }

        private void InitializeMessage() {
            message = new Messages.Mayfield.ChestLeds();
            SetAllLeds(pubColor);
        }

        private void SetAllLeds(Color cIn) {
            if (message.leds[0] == null) {
                for (int i = 0; i < 15; ++i) {
                    message.leds[i] = new Messages.Mayfield.Led((uint)(pubColor.r * 255), (uint)(pubColor.g * 255), (uint)(pubColor.b * 255));
                }
            }
            else {
                for (int i = 0; i < 15; ++i) {
                    message.leds[i].red = (uint)(pubColor.r * 255);
                    message.leds[i].green = (uint)(pubColor.g * 255);
                    message.leds[i].blue = (uint)(pubColor.b * 255);
                }
            }
        }

        IEnumerator ColorCycle() {
            float curTime = 0, totalTime = 2;

            while (curTime < totalTime) {
                float per = curTime / totalTime;
                pubColor = Color.HSVToRGB(per, 1, 1);
                PublishMessage();
                yield return new WaitForSeconds(Time.deltaTime);
        curTime = (float)(curTime % 1.9);
                curTime += Time.deltaTime;
            }
        }

        private void PublishMessage() {
            SetAllLeds(pubColor);
            Publish(message);
            Debug.Log(message);
        }


    }
}
