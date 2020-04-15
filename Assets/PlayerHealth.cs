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

        public void GotHit(float damage) {
            curHealth -= damage;
            StartCoroutine(RechargeAfterTime());
        }

        IEnumerator RechargeAfterTime() {
            if (rechargeCoroutineRunning) {
                // reset 
                curTimeUntilRecharge = rechargeTimeInterval;
                yield break;
            }
            curTimeUntilRecharge = rechargeTimeInterval;
            while (curTimeUntilRecharge > 0) {
                yield return null;
                Debug.Log(curHealth);
                curTimeUntilRecharge -= Time.deltaTime;
            }
            curHealth = 100;
            rechargeCoroutineRunning = false;
        }
    }
}
