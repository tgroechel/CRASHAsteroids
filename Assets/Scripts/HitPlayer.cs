using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPlayer : MonoBehaviour
{
    public int flashLength;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            count = 0;
            GetComponent<Image>().enabled = true;
        }
        if (count > flashLength)
        {
            GetComponent<Image>().enabled = false;
        }
        else
        {
            count += 1;
        }
        
    }

}
