using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject healthBarUI;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            //healthBarUI.setActive(true);
        }
        if (health <= 0)
        {
            //Destory(gameObject);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    float CalculateHealth()
    {
        return health / maxHealth;
    }
}
