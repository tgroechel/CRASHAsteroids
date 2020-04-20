using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float autoSpawnTime;
    public float autoNumberOfMinions;
    public float forwardOffset;
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

        // Initialize coroutineRunning bool to true to prevent autospawn (make this false here for auto spawning minions)
        coroutineRunning = true;

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
        if (!coroutineRunning && minionsAlive.Count < autoNumberOfMinions && animator.GetBool("Activate"))
        {
            coroutineRunning = true;
            StartCoroutine(AutoSpawnMinions());
        }   
    }

    private IEnumerator AutoSpawnMinions()
    {
        for(int i=0;i<autoNumberOfMinions - minionsAlive.Count;i++)
        {
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
        newMinion.transform.position = transform.position + (Camera.main.transform.forward.normalized * forwardOffset);

        // Return the newly spawned minion
        return newMinion;
    }
}
