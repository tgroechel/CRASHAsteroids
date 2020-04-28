using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float autoSpawnTime;
    public float autoNumberOfMinions;
    public float forwardOffset, randomOffsetSphereRadius;
    public bool autoSpawnMinions;
    public AudioClip minionSpawnSound;
    public float volume; 
    private List<GameObject> minionsAlive;
    private GameObject minionPrefab;
    private bool coroutineRunning;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Loading NavMesh prefab
        minionPrefab = Resources.Load<GameObject>(ResourcePathManager.prefabsFolder + ResourcePathManager.minion) as GameObject;

        // Get Animator component
        animator = GetComponent<Animator>();

        // Initialize coroutineRunning bool to false
        coroutineRunning = false;

        // Create new minionsAlive list
        minionsAlive = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Remove destroyed entries from minionsAlive
        minionsAlive.RemoveAll(item => item == null);

        // Create more minions if the number of minions alive is < numberOfMinions
        // and the boss is in activated state
        if (!coroutineRunning && minionsAlive.Count < autoNumberOfMinions && animator.GetBool("Activate") && autoSpawnMinions)
        {
            coroutineRunning = true;
            StartCoroutine(AutoSpawnMinions());
        }   
    }

    private IEnumerator AutoSpawnMinions()
    {
        for(int i=0;i<autoNumberOfMinions - minionsAlive.Count;i++)
        {
            // If boss got deactivated, wait till he gets activated
            while (!animator.GetBool("Activate"))
                yield return null;

            // Spawn a new minion
            GameObject newMinion = SpawnMinion();

            // Add this minion to the list of spawned minions
            minionsAlive.Add(newMinion);

            // Spawn a minion every spawnTime seconds
            yield return new WaitForSeconds(autoSpawnTime);
        }

        coroutineRunning = false;
    }

    public GameObject SpawnMinion()
    {
        // Instantiate new minion 
        GameObject newMinion = Instantiate(minionPrefab, transform.parent);

        // Position the minion w.r.t. the boss using offset vector determined by camera direction
        // (i.e. minion will be spawned on the side of the boss which is opposite to the camera)
        Vector3 minionPosition = transform.position + (Camera.main.transform.forward.normalized * forwardOffset);
        minionPosition.y = transform.position.y;

        // To give some randomness to minion positions (add this to the minionPosition determined above)
        Vector3 randomOffsetVector = Random.onUnitSphere * randomOffsetSphereRadius;
        minionPosition = minionPosition + randomOffsetVector;
        minionPosition.y = transform.position.y;

        // Finally, set the position of the newly spawned minion
        newMinion.transform.position = minionPosition;

        // Play minion spawn sound
        AudioSource.PlayClipAtPoint(minionSpawnSound, newMinion.transform.position, volume);

        // Return the newly spawned minion
        return newMinion;
    }
}
