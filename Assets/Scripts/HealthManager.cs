using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sid
{
    public class HealthManager : MonoBehaviour
    {
        public float health;
        Renderer objRenderer;
        Renderer[] childRenderers;
        Color objOriginalColor;
        List<Color> childOriginalColors;
        public static float damageTime;
        public static Color colorOnDamage = new Color(1, 0, 0);

        // Store original color of current object and its children (parts)
        void Awake()
        {
            health = 100;
            damageTime = Time.deltaTime * 3;

            // Initialising object renderer and its original color
            if (GetComponent<Renderer>() != null)
            {
                objRenderer = GetComponent<Renderer>();
                objOriginalColor = objRenderer.material.color;
            }
            else
            {
                objRenderer = null;
            }

            // Initialising child renderers and their original colors
            childRenderers = GetComponentsInChildren<Renderer>();
            childOriginalColors = new List<Color>();
            foreach (Renderer childRend in childRenderers)
            {
                childOriginalColors.Add(childRend.material.color);
            }
        }

        /*  If an enemy object is hit by a bullet, change its color for a brief period and decrease its health.
            If the enemy object has child objects, then change the color of each child.
            If health becomes <0, then deactivate the enemy. */
        public IEnumerator DecreaseHealth(float damage)
        {
            Time.timeScale = 1;
            if (health - damage <= 0)
            {
                gameObject.SetActive(false);
                FindObjectOfType<AudioManager>().Play("GameWin");
            }
            else
            {
                health -= damage;
                if (objRenderer != null)
                {
                    objRenderer.material.color = colorOnDamage;
                    yield return new WaitForSeconds(damageTime);
                    objRenderer.material.color = objOriginalColor;
                }
                else
                {
                    foreach (Renderer childRend in childRenderers)
                    {
                        childRend.material.color = colorOnDamage;
                    }
                    yield return new WaitForSeconds(damageTime);
                    int i = 0;
                    foreach (Renderer childRend in childRenderers)
                    {
                        childRend.material.color = childOriginalColors[i];
                        i++;
                    }
                }
            }
        }

        // Decrease health of the gameobject by 'damage' amount
        public void CallDecreaseHealth(float damage)
        {
            StartCoroutine(DecreaseHealth(damage));
        }
    }
}