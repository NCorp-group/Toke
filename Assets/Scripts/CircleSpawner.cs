using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class CircleSpawner : MonoBehaviour
{
    
    public GameObject spawn;

    [Space]
    public float radius = 1f;

    public int items = 5;
    public float delay = 0f;
    public float pulseDelay = 0f;
    public float offset = 0f;
    [Space]
    [Header("The force applied to object when spawned, normal to its angle.")]
    public Vector2 normalForce = Vector2.zero;
    public bool repeating = false;
    public int repetitions = 5;
    
    private Complex rotation = Complex.Zero;
    
    private delegate IEnumerator SpawnRoutine();

    private SpawnRoutine s;
    
    private void Start()
    {
        var rad = 2 * Mathf.PI / items;
        var angle = Mathf.Rad2Deg * rad;
        rotation = Complex.FromPolarCoordinates(1, angle);

        if (repeating)
        {
            s = new SpawnRoutine(SpawnForever);
        }
        else
        {
            s = new SpawnRoutine(SpawnN);
        }
        
        StartCoroutine(Begin());
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(offset);

        StartCoroutine(s());
    }
    
    private IEnumerator Spawn()
    {
        var pos2D = new Complex(transform.position.x + radius, transform.position.y);

        for (int i = 0; i < items; i++)
        {
            Debug.Log($"spawning object {i}");
            var pos = new UnityEngine.Vector3((float) pos2D.Real, (float) pos2D.Imaginary, 0f);
            Instantiate(spawn, pos, UnityEngine.Quaternion.identity);
            pos2D *= rotation;
            yield return new WaitForSeconds(pulseDelay);
        }

        // TODO: 
        // yield return new WaitForSeconds(0f);
    }

    private IEnumerator SpawnForever()
    {
        while (repeating)
        {

            StartCoroutine(Spawn());
            yield return new WaitForSeconds(delay);
        }
        
    }

    private IEnumerator SpawnN()
    {
        for (int i = 0; i < repetitions; i++)
        {
            StartCoroutine(Spawn());
            yield return new WaitForSeconds(delay);
        }
    }
    
}
