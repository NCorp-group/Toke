<<<<<<< HEAD
using System;
using System.Collections;
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> 2fe8c049131d32f2b0c9e0298e1df505871a2a10
using UnityEngine;

public class Projectile : MonoBehaviour
{
<<<<<<< HEAD
    public float acceleration = 0f;
    public int damage;
    [Header("if t = 0, then the projectile lives until it collides with something")]
    public float lifetime;
    public AOE aoe;
    
    public GameObject spawnObjectOnCollision;

    
    private Rigidbody2D rb;
    private Animator _animator;

    private float t = 0f;


    


    private Action checkTime;
    
    

    public void Setup(float lifetime, int damage)
    {
        this.damage = damage;
        this.lifetime = lifetime;
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (t == 0)
        {
            checkTime = () => { };
        }
        else
        {
            checkTime = () =>
            {
                t += Time.deltaTime;
                if (t > lifetime)
                {
                    DestroyProjectile();
                }
            };
        }
    }

    private void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        rb.velocity = rb.velocity + (rb.velocity * acceleration * Time.deltaTime);

        // checkTime();
        //var v = rb.velocity;
        //var angle = Utilities.GetAngleFromVectorFloat(v);
        
        //transform.Rotate(new Vector3(angle, 0f, 0));
        // rb.SetRotation(Quaternion.Euler(v));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"colliding with object {other.gameObject.name}");
        rb.isKinematic = true;
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        // other.rigidbody.velocity = Vector2.zero;
        _animator.SetTrigger("collision");

        var enemy = other.collider.GetComponent<Enemy>();
        var projectile_hit_enemy = enemy != null;
        if (projectile_hit_enemy)
        {
            
            enemy.TakeDamage(damage);
        }

        // Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerTarget>();
        var projectile_hit_player_character = player != null;
        if (projectile_hit_player_character)
        {
            // TODO: deal damage
            // player.Damage(damage);
        }

        var enemy = other.GetComponent<Enemy>();
        var projectile_hit_enemy = enemy != null;
        if (projectile_hit_enemy)
        {
            Debug.Log("hit an enemy");
            enemy.TakeDamage(damage);
        }
        
        Debug.Log($"tag of collider is = {other.gameObject.tag}");
        
        Debug.Log("colliding with object");
        
        /*
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            anim.SetTrigger("collision");
            rb.Sleep();
        }
        else
        {
            Debug.Log("DAMN i hit another projectile!!!");
        }
        */
        

        if (aoe != null)
        {
            Instantiate(aoe, transform.position, Quaternion.identity);
        }

        if (spawnObjectOnCollision != null)
        {
            Instantiate(spawnObjectOnCollision, transform.position, Quaternion.identity);
        }
        
        // StartCoroutine(DestroyProjectile());
    }

    private void DestroyProjectile()
    {
=======
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    public float lifeTime = 2f;
    public float velocity = 2f;

    void  Awake ()
    {
        Destroy(gameObject, lifeTime);
    }

    public GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision){
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.30f); // Destroy object after 5 seconds of hitting something
>>>>>>> 2fe8c049131d32f2b0c9e0298e1df505871a2a10
        Destroy(gameObject);
    }
}