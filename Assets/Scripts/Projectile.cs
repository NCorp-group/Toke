using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour
{
    public float acceleration = 0f;
    public int damage;

    private bool ignore = false;

    [Header("if t = 0, then the projectile lives until it collides with something")]
    public float lifetime = 1;
  
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
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        Assert.IsTrue(lifetime >= 0);

        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rb);
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity + (rb.velocity * acceleration * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision) {

        if (ignore)
        {
            return;
        }

        ignore = true;

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
                DestroyProjectile();
             }
        }
        
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.collider.gameObject.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                DestroyProjectile();
            }
        }
        else if (collision.collider.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.collider.gameObject.GetComponent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                if (animatorProvidesOnHitEffect)
                {
                    animator?.SetTrigger("collision");
                }
                else
                {
                    DestroyProjectile();
                }
            }
        }
    }
    
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}