using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour
{
    public float acceleration = 0f;
    public int damage;
    
    [Header("if t = 0, then the projectile lives until it collides with something")]
    public float lifetime;
  
    [Header("the color used to light up the player sceptre, when this projectile is equipped")]
    public Color color = Color.green;

    [Header("If set to true, the animator is responsible for destroying the projectile, using an animation event.")]
    [SerializeField] private bool animatorProvidesOnHitEffect = false;
    public GameObject spawnObjectOnCollision;

    public enum Variant
    {
        PLAYER,
        ENEMY
    }

    public Variant projectileType = Variant.PLAYER;
    

    public GameObject hitEffect;
    
    
    public float lifeTime = 2f;

    
    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        Assert.IsTrue(lifetime >= 0);

        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }

        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity + (rb.velocity * acceleration * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.CompareTag("Collidable"))
        {
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.30f);
            }
            
             // Destroy object after 5 seconds of hitting something
            
             animator?.SetTrigger("collision");
             // in case the
             if (animator != null && !animatorProvidesOnHitEffect)
             {
                 Destroy(gameObject);
             }
        }
        
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyProjectile();
            }
        }
    }
    
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}