using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    IEnumerator MoveCubeUp()
    {
        transform.localPosition = transform.localPosition + Vector3.up;
        yield return new WaitForSeconds(1f);
        transform.localPosition = transform.localPosition + Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(MoveCubeUp());
        }
    }
}
