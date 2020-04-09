using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMoveScript2 : MonoBehaviour
{

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    private Vector3 startPos;
    private Rigidbody rb;

    public float speed;
    public float accuracy;
    public float autoTimeout;

    private float timeElapsed;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        timeElapsed = 0;
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (speed != 0 && rb != null)
            rb.position += (transform.forward) * (speed * Time.deltaTime);

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= autoTimeout)
        {
            rb.position += new Vector3(100, 0, 0); // make it go away
            gameObject.SetActive(false);
        }
    }


    void OnCollisionEnter(Collision co)
    {
        if (hitSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(hitSFX);
        }

        rb.position += new Vector3(100, 0, 0); // make it go away

        timeElapsed = autoTimeout - 1; // so that the bullet will be active for one more second to allow the sound to play
    }

    public void playSpawnSound() {
        if (shotSFX != null && GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
    }

    public void SetDirection(Vector3 dir)
    {
        timeElapsed = 0;

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Quaternion rotation = Quaternion.LookRotation(dir);
        gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.rotation, rotation, 1);
    }
}
