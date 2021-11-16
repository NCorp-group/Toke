using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public GameObject[] objects;
    public float offset = 0f;
    public float period = 0f;

    private void OnDestroy()
    {
        var obj = objects[0];
        Instantiate(obj, transform.position, Quaternion.identity);
        /*
        
        StartCoroutine(Offset());
        
        foreach (var obj in objects)
        {
            StartCoroutine(Spawn(obj));
        }
        */
    }

    private IEnumerator Offset()
    {
        yield return new WaitForSeconds(offset);
    }

    private IEnumerator Spawn(GameObject obj)
    {
        Instantiate(obj, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(period);
    }
}
