using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSphereToCollisionPoin : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            InstanciateDebugCylider(contact);
        }

    }

    private void InstanciateDebugCylider(ContactPoint contact)
    {
        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = contact.point;
        cylinder.transform.rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
    }
}
