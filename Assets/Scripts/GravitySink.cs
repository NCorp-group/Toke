using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class GravitySink : MonoBehaviour
{
    [SerializeField] private LayerMask layersAffected;
    [SerializeField] private float mass = 10_000f;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float radius;
      
    private readonly float G = (float) 6.674e-11;

    void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
    }
    

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("an object has entered the gravitational zone.");
        
        var position = transform.position;
        var other_pos = other.transform.position;
        var distance = Vector2.Distance(other_pos, position);
        var m = other.attachedRigidbody.mass;
        var magnitude = G * (mass * m) / (distance * distance);
        Debug.Log($"applying a force with magnitude = {magnitude}");
        
        other.attachedRigidbody.AddForce((position - other_pos).normalized * magnitude ,ForceMode2D.Impulse);
    }

    private int n_objects_in_range;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        n_objects_in_range++;
        Debug.Log($"in range = {n_objects_in_range}");
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        n_objects_in_range--;
        Debug.Log($"in range = {n_objects_in_range}");
    }

    public int GetNumberOfBodiesAffected() => n_objects_in_range;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
