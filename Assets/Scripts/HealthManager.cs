using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

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
        private GameObject locationEffectsKuri;
        private NavMeshAgent agent;
        private bool kuriDestinationCalculated;

        // Slider for health bar
        Slider slider;
        public BehaviorTree behaviorTree;
        public GameObject kuri;

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

            // Get Location Effects for Kuri
            locationEffectsKuri = GameObject.Find("LocationEffects").transform.GetChild(1).gameObject;
            kuriDestinationCalculated = false;
        }

        private void Update()
        {
            // Logic to check if kuri has reached destination to kill the boss
            if (isBoss && !animator.GetBool("Activate"))
            {
                // Wait till kuriDestination is calculated
                StartCoroutine(WaitForKuriDestination());

                if(kuriDestinationCalculated)
                {
                    print("Kuri's distance from boss kill is ->" + Vector3.Distance(kuri.transform.position, EnemySoundsScript.kuriDestination));
                    if (Vector3.Distance(kuri.transform.position, EnemySoundsScript.kuriDestination) < 1.1f)
                    {
                        // Stop detination effects for Kuri
                        locationEffectsKuri.SetActive(false);
                        kuriDestinationCalculated = false;

                        // Play Game Win music if player just killed the boss
                        if (isBoss)
                            kuri.GetComponent<AudioManager>().Play("GameWin");

                        // Set isDead bool to true so that 'Death' animation is played
                        // Note: Make sure death animation is played last since after it completes, the boss object is destroyed
                        animator.SetBool("isDead", true);
                    }
                }
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
                    animator.SetBool("isDead", true);
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
            //yield return new WaitForSeconds(resurrectionTime);
            float timeLeft = resurrectionTime;
            while(timeLeft >=0)
            {
                timeLeft -= Time.deltaTime;
                string message = "Time Left Until Boss Reactivates:\n" + timeLeft;
                AlignAmmo.textComponent.SetText(message);
                yield return null;
            }
            AlignAmmo.textComponent.SetText("");

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

        // If Kuri collides with the boss in deactivated state, kill the boss
        /*public void OnTriggerEnter(Collider collider)
        {
            if(!animator.GetBool("Activate") && collider.gameObject.name == "Kuri")
            {
                // Set isDead bool to true so that 'Death' animation is played
                animator.SetBool("isDead", true);

                // Play Game Win music if player just killed the boss
                if (isBoss)
                    collider.gameObject.GetComponent<AudioManager>().Play("GameWin");
            }
        }*/

        // Destroy the object
        // Note: This function is called after the Death animation finishes playing
        void DestroyEnemy()
        {
            Destroy(gameObject);
        }

        public IEnumerator WaitForKuriDestination()
        {
            while (!EnemySoundsScript.kuriDestinationCalculated)
            {
                yield return null;
            }
            kuriDestinationCalculated = true;
        }
    }
}