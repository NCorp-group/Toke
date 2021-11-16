/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 2f;

    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;

    private Seeker seeker;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        sr = GetComponent<SpriteRenderer>();
        
        InvokeRepeating(nameof(UpdatePath), 0f, 1f);
        
    }

    private void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    
    void FixedUpdate()
    {
        if (path != null)
        {
            reachedEndOfPath = currentWaypoint >= path.vectorPath.Count;
            if (reachedEndOfPath) return;
            
            
            var direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            var force = direction * speed * Time.deltaTime;

            rb.AddForce(force);
            
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }

            var x = rb.velocity.x;
            if (x >= 0.01f)
            {
                sr.flipX = false;
                Debug.Log("facing right");
            }
            else if (x <= -0.01f)
            {
                sr.flipX = true;
                Debug.Log("facing left");
            }

        }
        
    }
}
*/