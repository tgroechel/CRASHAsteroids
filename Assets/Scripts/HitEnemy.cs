using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sid
{
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

        // For bullets triggered by player
        private void OnCollisionEnter(Collision other)
        {
            /* If HealthManager script exists on the collided object then
               we assume it is an enemy and we decrease its health,
               else the bullet simply vanishes upon collision */
            if (other.gameObject.GetComponent<HealthManager>() != null)
            {
                print("Name of gameobject entered is " + other.gameObject.name);
                HealthManager healthManager = other.gameObject.GetComponent<HealthManager>();
                healthManager.CallDecreaseHealth(bulletDamage);
            }
        }
    }
}