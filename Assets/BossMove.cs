using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    bool isMoving = false;
    bool forward = true;
    float fwdAmt = 3f;
    float sideAmt = 3f;
    public AnimationCurve animCurve;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator MoveMe()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;
        forward = !forward;
        Vector3 direction = Vector3.back;
        if (forward)
        {
            sideAmt = Random.Range(2, 3);
            fwdAmt = Random.Range(0, 3);
            direction = Vector3.left * sideAmt + Vector3.back * fwdAmt;
        }
        else
        {
            direction = Vector3.right * sideAmt + Vector3.forward * fwdAmt;
        }

        Vector3 targetPosition = transform.localPosition + direction;
        Vector3 origPosition = transform.localPosition;
        float timePassed = 0;
        float totalTime = 0.5f;
        while (timePassed < totalTime)
        {
            float percentDone = timePassed / totalTime;
            // transform.localPosition = Vector3.Lerp(origPosition, targetPosition, animCurve.Evaluate(percentDone));
            transform.localPosition = Vector3.Slerp(origPosition, targetPosition, animCurve.Evaluate(percentDone));
            yield return new WaitForSeconds(Time.deltaTime);
            timePassed += Time.deltaTime;
        }

        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(MoveMe());
    }
}
