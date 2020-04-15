using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crash {
    public class PlayerHealth : MonoBehaviour {
        public const float MAX_PLAYER_HEALTH = 100;
        public float curHealth = 100;
        public float rechargeTimeInterval = 5;
        private float curTimeUntilRecharge;
        public bool rechargeCoroutineRunning = false;

        PlayerEffectPlane playerEffectPlane;
        private void Awake() {
            playerEffectPlane = transform.GetComponentInChildren<PlayerEffectPlane>();
        }


        public void GotHit(float damage) {
            SetPlayerHealth(curHealth - damage);
            StartCoroutine(RechargeAfterTime());
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
                Debug.Log(curHealth);
                curTimeUntilRecharge -= Time.deltaTime;
            }
            SetPlayerHealth(MAX_PLAYER_HEALTH);
            rechargeCoroutineRunning = false;
        }


        public void SetPlayerHealth(float newHealth) {
            curHealth = newHealth;
            UpdatePlaneEffectColor();
        }

        public void UpdatePlaneEffectColor() {
            playerEffectPlane.SetTransparency((MAX_PLAYER_HEALTH - curHealth) / MAX_PLAYER_HEALTH);
        }
    }
}
