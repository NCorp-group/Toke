using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class VolleyAttack : MonoBehaviour
{
    public Projectile[] projectiles;
    [Header("The angle between each spawned projectile in degrees")]
    public float spawnAngle = 20f;

    [Space]
    [Header("The time (in seconds) between each spawned projectile. 0 creates an instantaneous volley.")]
    public float spawnDelayBetweenEach = 0f;
    
    public float speed = 10f;
    [Space]
    public Transform firepoint;

    public bool spawnInClockwiseOrder = true;
    
    private Animator anim;
    private SpriteRenderer sr;
    private float _angle;
    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        _angle = spawnAngle;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("attack"); // projectile fired using animation event
            // StartCoroutine(FireProjectile());
        }
    }

    private void FireVolley()
    {
        StartCoroutine(_FireVolley());
    }

    private IEnumerator _FireVolley()
    {
        var pos = firepoint.position;
        var offset = firepoint.position.x - transform.position.x;
        var facing_left = !sr.flipX;

        if (facing_left)
        {
            offset *= -1;
        }

        offset *= -1;
        
        // mouse position
        Vector3 mousePos = Input.mousePosition;   
        mousePos.z=Camera.main.nearClipPlane;
        Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
        
        var projectile_spawn_pos = new Vector2(transform.position.x + offset, transform.position.y);
        // var projectile_spawn_pos = new Vector2(transform.position.x, transform.position.y);
        
        
        
        foreach (var projectile in projectiles)
        {
            var projectile_direction = (Worldpos2D - projectile_spawn_pos).normalized;
            var angle = Utilities.GetAngleFromVectorFloat(new Vector3(projectile_direction.x, projectile_direction.y, 0));
            
            Debug.Log($"angle is {angle}");
            var spawning_angle = Quaternion.Euler(0, 0, angle + _angle);
            Debug.Log($"Spawning angle is {spawning_angle}");
            var instance = Instantiate(projectile, projectile_spawn_pos, spawning_angle);
            // instance.Setup(lifetime, 20);
            _angle += angle;
            // disable collider momentarily to allow projectiles to spread a bit so they. don't collide with each other.
            StartCoroutine(MomentarilyDisableCollider2D(instance.GetComponent<Collider2D>(), 0.5f));

            var ahhh = Quaternion.Euler(_angle, 0, 0) * new Vector3(projectile_direction.x, projectile_direction.y, 0);
            var ahhh2D = new Vector2(ahhh.x, ahhh.y); 
            
            instance.GetComponent<Rigidbody2D>().AddForce(ahhh * speed, ForceMode2D.Impulse);

            
//            instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * spawning_angle * speed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(spawnDelayBetweenEach);
        }

        _angle = spawnAngle;
    }

    private IEnumerator MomentarilyDisableCollider2D(Collider2D collider, float t)
    {
        collider.enabled = false;
        yield return new WaitForSeconds(t);
        collider.enabled = true;
    }
    
    
    
}
