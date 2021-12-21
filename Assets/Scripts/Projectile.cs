using TMPro.Examples;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class Projectile : MonoBehaviour
{
    // Projectile's base stats
    public float acceleration = 0f;
    public float damage = 0;
    public float speed = 1;
    [Header("if t = 0, then the projectile lives until it collides with something")]
    public float lifetime = 1;


    private bool collided = false;
  
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
    private static readonly int Collision1 = Animator.StringToHash("collision");

    private void Start()
    {
        Assert.IsTrue(lifetime >= 0);

        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
    }
    
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
        Vector2 velocity = rb.velocity;
        rb.velocity += acceleration * Time.deltaTime * velocity;
        transform.rotation = Quaternion.Euler(0f, 0f, Util.GetAngleFromVectorFloat(new Vector3(velocity.x, velocity.y, 0f)));
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
        //float? damage = null,
        float? accelerationMult = null,
        //float? lifetime = null,
        float? damageMult = null,
        float? lifetimeMult = null
    )
    {
        gameObject.layer = variant switch
        {
            Variant.ENEMY => LayerMask.NameToLayer("Enemy Projectiles"),
            Variant.PLAYER => LayerMask.NameToLayer("Player Projectiles")
        };

        damage *= damageMult ?? 1;
        lifetime *= lifetimeMult ?? 1;
        acceleration *= accelerationMult ?? 1;
        //this.damage = damage != null ?? this.damage;
        //this.acceleration = acceleration ?? this.acceleration;
        //this.lifetime = lifetime ?? this.lifetime;
        //this.damage = damageMultiplier != null ? (int) (this.damage * damageMultiplier) : this.damage;
        //this.lifetime = lifetimeMultiplier != null ? (this.lifetime * lifetimeMultiplier?? 1) : this.lifetime;
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
        
        if (collided)
        {
            return;
        }

        collided = true;

        if (collision.collider.CompareTag("Collidable"))
        {
            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(effect, 0.30f);
            }
            
             // Destroy object after 5 seconds of hitting something
            
             //Debug.Log("animate: COLLISION");
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
                //Debug.Log($"enemy taking damage: {damage}");
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
            //Debug.Log("DO WE GET HERE??");
            // animator.SetTrigger("collision");
            //Debug.Log("Hit Player with projectile");
            var playerHealth = collision.collider.gameObject.GetComponent<PlayerHealthController>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                if (animatorProvidesOnHitEffect)
                {
                    //Debug.Log("animate: COLLISION");
                    if (animator is Animator a)
                    {
                        //Debug.Log("ANIMATOR IS HERE");
                        a.SetTrigger(Collision1);
                    }
                    //animator?.SetTrigger("collision");
                }
                else
                {
                    DestroyProjectile();
                }
            }
        }
        else if (collision.collider.gameObject.CompareTag("Boss"))
        {
            
            var bhc = collision.collider.gameObject.GetComponent<BossHealthController>();
            if (bhc is not null)
            {
                Debug.Log("Hit a boss");
                bhc.TakeDamage(damage);
                if (animatorProvidesOnHitEffect)
                {
                    //Debug.Log("animate: COLLISION");
                    if (animator is Animator a)
                    {
                        //Debug.Log("ANIMATOR IS HERE");
                        a.SetTrigger(Collision1);
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

    public class Type
    {
        // Types of projectiles
        public const string BOLT = "bolt";
        public const string CHARGED = "charged";
        public const string FIRE_BALL = "fire ball";
        public const string CROSSED = "crossed";
        public const string ICE_SHARD = "ice shard";
        public const string LIGHTNING_BIRD = "lightning bird";
        public const string PINK_SPARK = "pink spark";
        public const string PULSE = "pulse";
        public const string SPARK = "spark";
        public const string WATER_BALL = "water ball";
        public const string WIND_ARC = "wind arc";
    }

}