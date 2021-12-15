using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    public float acceleration = 0f;
    public int damage;
    public float speed = 3f;

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

    public GameObject hitEffect = null;
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
        Assert.IsNotNull(animator);
    }
    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        rb.velocity += (rb.velocity * acceleration * Time.deltaTime);
    }

    /// <summary>
    /// Constructor method
    /// </summary>
    /// <param name="variant"></param>
    /// <param name="damage"></param>
    /// <param name="acceleration"></param>
    /// <param name="lifetime"></param>
    /// <param name="damageMultiplier"></param>
    /// <param name="lifetimeMultiplier"></param>
    public void Setup(
        Variant variant,
        int? damage = null,
        float? acceleration = null,
        int? lifetime = null,
        float? damageMultiplier = null,
        float? lifetimeMultiplier = null
    )
    {
        gameObject.layer = variant switch
        {
            Variant.ENEMY => LayerMask.NameToLayer("Enemy Projectiles"),
            Variant.PLAYER => LayerMask.NameToLayer("Player Projectiles")
        };
        this.damage = damage ?? this.damage;
        this.acceleration = acceleration ?? this.acceleration;
        this.lifetime = lifetime ?? this.lifetime;
        this.damage = damageMultiplier != null ? (int) (this.damage * damageMultiplier) : this.damage;
        this.lifetime = lifetimeMultiplier != null ? (int) (this.lifetime * lifetimeMultiplier) : this.lifetime;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        rb.velocity = new Vector2(0, 0);
        if (GetComponent<Light2D>() is Light2D light)
        {
            light.volumeIntensityEnabled = false;
        }
        else if (GetComponentInChildren<Light2D>() is Light2D l)
        {
            l.volumeIntensityEnabled = false;
        }
        
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
            
             Debug.Log("animate: COLLISION");
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
                if (animatorProvidesOnHitEffect)
                {
                    animator.SetTrigger("collision");
                }
                else
                {
                    DestroyProjectile();
                }
                
            }
        }
        else if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("DO WE GET HERE??");
            animator.SetTrigger("collision");
            Debug.Log("Hit Player with projectile");
            var playerHealth = collision.collider.gameObject.GetComponent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                if (animatorProvidesOnHitEffect)
                {
                    Debug.Log("animate: COLLISION");
                    if (animator is Animator a)
                    {
                        Debug.Log("ANIMATOR IS HERE");
                        a.SetTrigger("collision");
                    }
                    //animator?.SetTrigger("collision");
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