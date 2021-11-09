using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject enemy;
    public int instances;
    public float spawnPeriod;
    public float offset;

    private int spawnCount;
    
    // Start is called before the first frame update
    private void Start()
    {
        spawnCount = 0;
        InvokeRepeating(nameof(Spawn), offset, spawnPeriod);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void Spawn()
    {
        if (spawnCount <= instances)
        {
            Instantiate(enemy, spawnLocation.position, spawnLocation.rotation);
        }

        spawnCount++;
    }
}
