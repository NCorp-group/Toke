using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    public Projectile projectile;
    
    public Transform target;
    public float firerate = 2f;
    public float speed = 5f;
    public float distance = 20f;

    private Animator anim;
    private SpriteRenderer sr;

    
    private float t = 0f;
    private RaycastHit2D response;
    private Transform firepoint;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        firepoint = transform.Find("Firepoint");
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= firerate)
        {
            t = 0;
            // TODO: use layer mask
            var direction = (target.position - firepoint.position).normalized;
            //var hit = Physics2D.Raycast(firepoint.position, firepoint.TransformDirection(direction), distance);
            var hit = Physics2D.Raycast(firepoint.position, direction, distance);
            Debug.Log("I hit something");
            if (hit)
            {
                // Debug.DrawRay(firepoint.position, firepoint.TransformDirection(hit.transform.position) * 50, Color.red, 2);
                response = hit;
                // anim.SetTrigger("fire");
                Fire();
                
                if (hit.transform.CompareTag("Player"))
                {
                    response = hit;
                    // anim.SetTrigger("fire");
                    Fire();
                }
            }
            
        }
    }

    // called by animation event
    private void Fire()
    {
        Debug.Log("I am in the fire");
        var direction = (response.transform.position - firepoint.position ).normalized;
        var angle = Utilities.GetAngleFromVectorFloat(direction);
        var p = Instantiate(projectile, firepoint.position, Quaternion.Euler(0, 0, angle));
        p.GetComponent<Rigidbody2D>()?.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
