using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciateAtRaycastHitPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button.");
            GetRayCastHitPoint();
        }


        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");


        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed middle click.");
        }
    }

    public void GetRayCastHitPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.name == "Floor")
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = hit.point;
                cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
        }
        else
        {
            Debug.Log("Did not Hit");
        }
    }

}
