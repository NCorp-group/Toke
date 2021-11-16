using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawn;



public class SpawnManager : MonoBehaviour, ISpawnable
{
    [Space]
    [Header("   Use any object which implements ISpawnable   ")]
    [Header("   Since Unity does not support interfaces in the Inspector   ")]
    [Space]
    public GameObject[] spawns;
    public Transform spawningPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        if (spawns == null) Destroy(gameObject);

        SpawnObj(transform);
        
        // all objects spawned goodnight ;-)
        // Destroy(gameObject);
    }

    private IEnumerator SpawnAll()
    {
        foreach (var obj in spawns)
        {
            var spawn = obj.GetComponent<ISpawnable>();
            if (spawn == null)
            {
                Debug.Log("ERROR object given does NOT implement ISpawnable");
                continue;
            }
            
            spawn.SpawnObj(spawningPoint);
            var delay = spawn.Delay();
            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnObj(Transform spawningPoint)
    {
        StartCoroutine(SpawnAll());
    }

    public float Delay()
    {
        return 0;
    }
}
