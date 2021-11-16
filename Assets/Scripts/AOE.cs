using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public int damage = 0;
    
    private Collider2D collider;
    private float t = 0f;
    
    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<Enemy>();
        var projectile_hit_enemy = enemy != null;
        if (projectile_hit_enemy)
        {
            Debug.Log("hit an enemy");
            enemy.TakeDamage(damage);
        }
    }

    // call in animation event, when the AOE should take effect
    private void EnableCollider()
    {
        collider.enabled = true;
    }

    // Destroy using Animation Event
    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
    
}
