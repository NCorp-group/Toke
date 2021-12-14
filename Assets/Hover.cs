using System;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public bool horizontal = false;
    public bool vertical = true;
    public float frequency = 0.1f;
    public float amplitude = 0.05f;
    public float offset = 0f;
    
    private readonly float _TAU = 2 * Mathf.PI;
    
    private void Update()
    {
        var t = Time.time;
        var arg = frequency * t * _TAU;
        transform.position +=
            new Vector3(
                horizontal ? (float) Math.Cos(arg) * amplitude + offset : 0f,
                vertical ? (float) Math.Sin(arg) * amplitude + offset : 0f,
                0f
            );
    }
}
