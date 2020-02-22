using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float health;
    Renderer objRenderer;
    Renderer[] childRenderers;
    Color objOriginalColor;
    List<Color> childOriginalColors;
    public static float damageTime;
    public static Color colorOnDamage = new Color(0.58f, 0, 0);

    // Start is called before the first frame update
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
        foreach(Renderer childRend in childRenderers)
        {
            print("Org Color of " + childRend.gameObject.name + " is " + childRend.material.color);
            childOriginalColors.Add(childRend.material.color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // If an enemy object is hit by a bullet, change its color for a brief period and decrease its health.
    // If the enemy object has child objects, then change the color of each child.
    // If health becomes <0, then deactivate the enemy.
    public IEnumerator DecreaseHealth(float damage, GameObject bullet)
    {
        Time.timeScale = 1;
        if (health - damage <= 0)
        {
            gameObject.SetActive(false);
            bullet.SetActive(false);
        }
        else
        {
            health -= damage;
            if(objRenderer != null)
            {
                objRenderer.material.color = colorOnDamage;
                yield return new WaitForSeconds(damageTime);
                print("I am also here!");
                objRenderer.material.color = objOriginalColor;
            }
            else
            {
                foreach (Renderer childRend in childRenderers)
                {
                    print("Changed color!");
                    childRend.material.color = colorOnDamage;
                }
                yield return new WaitForSeconds(damageTime);
                var i = 0;
                print("I am here!");
                foreach (Renderer childRend in childRenderers)
                {
                    print("Setting Org Color of " + childRend.gameObject.name + " to " + childOriginalColors[i]);
                    childRend.material.color = childOriginalColors[i];
                    i++;
                }
            }
            
            bullet.SetActive(false);
        }
    }
}
