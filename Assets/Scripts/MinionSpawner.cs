using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    public float spawnTime;
    public float numberOfMinions;
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
        if (!coroutineRunning && minionsAlive.Count < numberOfMinions && animator.GetBool("Activate"))
        {
            coroutineRunning = true;
            StartCoroutine(SpawnMinions());
        }

        // for debugging
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Number of Minions Alive: " + minionsAlive.Count);
            int i = 0;
            foreach(GameObject minion in minionsAlive)
            {
                i++;
                print("Minion No. "+ i +" name is -> " + minion.name);
            }
        }
            
    }

    private IEnumerator SpawnMinions()
    {
        for(int i=0;i<numberOfMinions - minionsAlive.Count;i++)
        {
            // Instantiate new minion 
            GameObject newMinion = Instantiate(minionPrefab, transform.parent);

            // Position the minion w.r.t. the boss using offset
            newMinion.transform.position = transform.position + (transform.forward.normalized * forwardOffset);

            // Add this minion to the list of spawned minions
            minionsAlive.Add(newMinion);

            // Spawn a minion every spawnTime seconds
            yield return new WaitForSeconds(spawnTime);
        }

        coroutineRunning = false;
    }
}
