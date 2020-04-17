using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Crash {
    public class PlayerHealth : MonoBehaviour {
        public const float MAX_PLAYER_HEALTH = 100;
        public float curHealth = 100;
        public float rechargeTimeInterval = 5;
        private float curTimeUntilRecharge;
        public bool rechargeCoroutineRunning = false, justHit = false;

        PlayerEffectPlane playerEffectPlane;
        Slider healthBarSlider;
        private void Awake() {
            playerEffectPlane = transform.GetComponentInChildren<PlayerEffectPlane>();
            healthBarSlider = transform.parent.GetComponentInChildren<Slider>();
        }


        public void GotHit(float damage) {
            if (!justHit) {
                SetPlayerHealth(curHealth - damage);
                StartCoroutine(JustHit());
            }
            StartCoroutine(RechargeAfterTime());
        }

        IEnumerator JustHit() {
            justHit = true;
            yield return new WaitForSeconds(0.5f);
            justHit = false;
        }

        IEnumerator RechargeAfterTime() {
            if (rechargeCoroutineRunning) {
                // reset
                curTimeUntilRecharge = rechargeTimeInterval;
                yield break;
            }
            rechargeCoroutineRunning = true;
            curTimeUntilRecharge = rechargeTimeInterval;
            while (curTimeUntilRecharge > 0) {
                yield return null;
                //  Debug.Log(curHealth);
                curTimeUntilRecharge -= Time.deltaTime;
            }
            SetPlayerHealth(MAX_PLAYER_HEALTH);
            rechargeCoroutineRunning = false;
        }


        public void SetPlayerHealth(float newHealth) {
            curHealth = newHealth;
            UpdatePlaneEffectColor();
            healthBarSlider.value = curHealth / MAX_PLAYER_HEALTH;
        }

        public void UpdatePlaneEffectColor() {
            playerEffectPlane.SetTransparency((MAX_PLAYER_HEALTH - curHealth) / MAX_PLAYER_HEALTH);
        }
    }
}
