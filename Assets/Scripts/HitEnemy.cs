using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    // Default, can be changed using GUI for different bullets
    public float bulletDamage = 10;
    public Collider objCollider;
    public Rigidbody objRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        objRigidbody = gameObject.GetComponent<Rigidbody>();
        objCollider = gameObject.GetComponent<Collider>();
        //objCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // For bullets triggered by player
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Caonima!!!!!");
        // If HealthManager script exists on the collided object then
        // we assume it is an enemy and we decrease its health,
        // else the bullet simply vanishes upon collision
        if (other.gameObject.GetComponent<HealthManager>() != null)
        {
            print("HealthManager present!");
            var healthManager = other.gameObject.GetComponent<HealthManager>();
            healthManager.CallDecreaseHealth(bulletDamage, gameObject);
            //gameObject.SetActive(false);
        }
        else
        {
            print("Trigger entered: " + other.gameObject.name);
            //gameObject.SetActive(false);
        }
    }
}
