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
            GameObject rootGameObject = other.gameObject;
            //print("Name of gameobject entered before HealthManager check is " + other.gameObject.name);
            if (rootGameObject.GetComponent<HealthManager>() != null)
            {
                print("Name of gameobject entered is " + rootGameObject.name);
                HealthManager healthManager = rootGameObject.GetComponent<HealthManager>();
                healthManager.CallDecreaseHealth(bulletDamage);
            }
        }
    }
}