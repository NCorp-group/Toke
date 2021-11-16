using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ThunderstrikeChain : MonoBehaviour
{
    public Vector2 direction = Vector2.right;
    public float distance = 2f;
    public float delay = 1f;

    private SpriteRenderer sr;

    private List<GameObject> spawnedThunderStrikes = new List<GameObject>();

    public GameObject thunderstrike;

    private int n_objects_to_spawn = 0;
    
    private Vector2 increment = Vector2.zero;

    private bool spawning = false;
    
    private void Start()
    {
        sr = thunderstrike.GetComponent<SpriteRenderer>();

        direction.Normalize();
        
        var width = sr.bounds.size.x;
        var height = sr.bounds.size.y;

        increment = sr.bounds.size * direction;
        
        Debug.Log($"sprite dimensions are width: {width} and height: {height}");
        
        var hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), distance);
        if (hit)
        {
            Debug.Log($"distance to target hit is {hit.distance}");
            hit.transform.GetComponent<SpriteRenderer>().color = Color.cyan;
            n_objects_to_spawn = Mathf.FloorToInt(hit.distance / width);
            Debug.Log($"{n_objects_to_spawn} can be spawned");
        }

        // StartCoroutine(Spawn());

        StartCoroutine(DoItAgain());

        // StartCoroutine(CleanUp());
    }

    private IEnumerator DoItAgain()
    {
        while (true)
        {
            StartCoroutine(Spawn());
            // StartCoroutine(CleanUp());
            yield return new WaitUntil((() => !spawning));
        }
    }

    private IEnumerator Spawn()
    {
        spawning = true;
        var _increment = increment;
        for (int i = 0; i < n_objects_to_spawn; i++)
        {
            var spawningPoint = transform.position + new Vector3(_increment.x, _increment.y, 0f);
            _increment += increment;
            //var obj = Instantiate(thunderstrike, spawningPoint, Quaternion.identity);
            Instantiate(thunderstrike, spawningPoint, Quaternion.identity);
            // spawnedThunderStrikes.Add(obj);
            yield return new WaitForSeconds(delay);
        }

        spawning = false;
    }

    private IEnumerator CleanUp()
    {
        yield return new WaitUntil((() => spawnedThunderStrikes.Count == n_objects_to_spawn));
        foreach (var obj in spawnedThunderStrikes)
        {
            Destroy(obj);
        }

        spawnedThunderStrikes = new List<GameObject>();
    }
    

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), 10f);

            if (hit)
            {
                Debug.Log($"distance to target hit is {hit.distance}");
                hit.transform.GetComponent<SpriteRenderer>().color = Color.cyan;
            }
        }
    }
}
