using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;

namespace Sid {
    public class HealthManager : MonoBehaviour {

        public float MAXHEALTH;
        float currentHealth;
        GameObject rootGameObject;
        Renderer objRenderer;
        Renderer[] childRenderers;
        Color objOriginalColor;
        List<Color> childOriginalColors;
        public static float damageTime;
        public static Color colorOnDamage = new Color(1, 0, 0);
        public bool isBoss;
        public float resurrectionTime;
        public float resurrectedHealthPercentage;
        public float phase2HealthPercentage;
        private Animator animator;

        // Slider for health bar
        Slider slider;
        public BehaviorTree behaviorTree;

        // Store original color of current object and its children (parts)
        void Awake() {
            animator = GetComponent<Animator>();
            slider = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();
            //behaviorTree = GetComponent<BehaviorTree>(); //TODO get this properly
            currentHealth = MAXHEALTH;
            slider.value = currentHealth / MAXHEALTH;
            damageTime = Time.deltaTime * 3;
            rootGameObject = gameObject;

            // Initialising object renderer and its original color
            if (rootGameObject.GetComponent<Renderer>() != null) {
                objRenderer = rootGameObject.GetComponent<Renderer>();
                objOriginalColor = objRenderer.material.color;
            }
            else {
                objRenderer = null;
            }

            // Initialising child renderers and their original colors
            childRenderers = rootGameObject.GetComponentsInChildren<Renderer>();
            childOriginalColors = new List<Color>();
            foreach (Renderer childRend in childRenderers) {
                childOriginalColors.Add(childRend.material.color);
            }
        }

        /*  If an enemy object is hit by a bullet, change its color for a brief period and decrease its health.
            If the enemy object has child objects, then change the color of each child.
            If health becomes <0, then deactivate the enemy. */
        public IEnumerator DecreaseHealth(float damage) {
            Time.timeScale = 1;
            if (currentHealth - damage <= 0) {

                if (!isBoss) {
                    // Make slider value zero and destroy the enemy from the scene
                    slider.value = 0;
                    Destroy(gameObject);
                }
                else {
                    // Make slider value zero and deactivate the enemy using animation
                    // Note: Boss can still be seen in the scene
                    // and automatically reactivates after some time
                    slider.value = 0;

                    // Check to ensure that a bullet does not affect
                    // the boss in deactivated state
                    if(animator.GetBool("Activate")){
                        animator.SetBool("Activate", false);
                        CallResurrectionTime();
                    }
                    
                }
                
                FindObjectOfType<AudioManager>().Play("GameWin");
            }
            else {
                if (currentHealth > MAXHEALTH / 2 && currentHealth - damage < MAXHEALTH / 2)
                {
                    behaviorTree?.SendEvent<object>("HalfHealth",currentHealth);
                }
                currentHealth -= damage;
                slider.value = currentHealth / MAXHEALTH;
                if (objRenderer != null) {
                    objRenderer.material.color = colorOnDamage;
                    yield return new WaitForSeconds(damageTime);
                    objRenderer.material.color = objOriginalColor;
                }
                if (childRenderers.Length > 0) {
                    foreach (Renderer childRend in childRenderers) {
                        childRend.material.color = colorOnDamage;
                    }
                    yield return new WaitForSeconds(damageTime);
                    int i = 0;
                    foreach (Renderer childRend in childRenderers) {
                        childRend.material.color = childOriginalColors[i];
                        i++;
                    }
                }
            }
        }

        /* Sets the Activate bool to true after resurrection time is complete */
        public IEnumerator ResurrectionTime()
        {
            yield return new WaitForSeconds(resurrectionTime);

            // Refill health of boss
            currentHealth = MAXHEALTH * resurrectedHealthPercentage / 100;
            slider.value = currentHealth / MAXHEALTH;

            // Reactivate the boss
            animator.SetBool("Activate", true);
        }

        // Decrease health of the gameobject by 'damage' amount
        public void CallDecreaseHealth(float damage) {
            StartCoroutine(DecreaseHealth(damage));
        }

        // Wait for some time (resurrection time) before reviving dead boss
        public void CallResurrectionTime()
        {
            StartCoroutine(ResurrectionTime());
        }
    }
}