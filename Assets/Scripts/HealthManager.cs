using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    float health;
    Renderer rend;
    Color originalColor;
    static Color colorOnDamage = new Color(0.58f, 0, 0);
    static float timeForDamage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        rend = transform.GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DecreaseHealth(float damage)
    {
        health -= damage;
        rend.material.color = colorOnDamage;
        yield return new WaitForSeconds(timeForDamage);
        rend.material.color = originalColor;
    }
}
