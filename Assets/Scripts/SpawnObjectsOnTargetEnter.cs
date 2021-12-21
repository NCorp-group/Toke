using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnObjectsOnTargetEnter : MonoBehaviour
{
    public event Action OnObjectsSpawned;

    public Transform target;
    public LayerMask targetLayerMask;

    public Transform spawnPoint;
    public Transform[] objectsToSpawn;

    private void Awake()
    {
        gameObject.layer = targetLayerMask;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Something entered area with name {other.gameObject.name}");
        
        if (other.gameObject.name == target.gameObject.name)
        {
            
            foreach (var t in objectsToSpawn)
            {
                Instantiate(t, spawnPoint.position, Quaternion.identity);
            }
            
            OnObjectsSpawned?.Invoke();
            
            Destroy(gameObject);
        }
    }
}
