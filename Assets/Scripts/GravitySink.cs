using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySink : MonoBehaviour
{
    [Header("should be positive, for a repulsive effect set repulsion to true")]
    public float mass = 1f;

    public bool repulsion = false;
    public bool changeFieldDirection = false;
    public float period = 5f;
    
    private readonly float G = 6.6743e-11f; // gravitational constant
    private CapsuleCollider2D collider;
    private Animator animator;
    
    private float t = 0f;
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Projectile")  
        {
            Debug.Log("A projectile has entered the gravity well");
            var pos = other.transform.position;
            var distance = Vector3.Distance(transform.position, pos);
            var dir = (pos - transform.position).normalized;

            var rb = other.GetComponent<Rigidbody2D>();
            var _mass = repulsion ? -mass : mass;
            var force = GravitationalForce(_mass, rb.mass, distance);
            Debug.Log($"force applied is {force}");
            var dir_inwards = -dir;
            rb.AddForce(dir_inwards * force, ForceMode2D.Impulse);
            var v = rb.velocity;
            var a = Utilities.GetAngleFromVectorFloat(v);
            other.transform.rotation = Quaternion.Euler(0, 0, a);
            
            //var angle = Utilities.GetAngleFromVectorFloat(dir);
            //other.transform.rotation = Quaternion.Euler(0, 0, angle);
            // other.transform.rotation = Quaternion.LookRotation(dir);
            // rb.SetRotation(Quaternion.Euler(dir));
        }
    }

    // https://en.wikipedia.org/wiki/Newton%27s_law_of_universal_gravitation
    private float GravitationalForce(float m1, float m2, float r)
    {
        return G * m1 * m2 / Mathf.Pow(r, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (changeFieldDirection && t >= period)
        {
            repulsion = !repulsion;
            animator.SetBool("repulsion", repulsion);
            t = 0;
        }
    }
}
