using System;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    private float old_fireRate;
    private float old_fireRateMultiplier;
    private float effectiveFireRate;
    [SerializeField] public float fireRate = 8;
    private float shotDelay;
    [SerializeField] private float counter;
    // Start is called before the first frame update

    public static event Action<Color> OnProjectileChanged;
    public static event Action<Color> OnProjectileSetColor;
    public Transform shootingPoint;
    public Projectile projectile;

    public static event Action OnFire;

    // Projectile's added stats
    private float projectileLifeMultiplier = 1;
    private float projectileSpeedMultiplier = 1;
    private float damageMultiplier = 1;
    private float fireRateMultiplier = 1;

    private void OnFireRateMultiplierChangedCB(float newFireRateMultiplier)
    {
        fireRateMultiplier = newFireRateMultiplier;
    }

    private void OnProjectileLifeMultiplierChangedCB(float newLifeTime) // CB is CallBack
    {
        projectileLifeMultiplier = newLifeTime;
    }

    private void OnDamageMultiplierChangedCB(float newDamageMultiplier)
    {
        //Debug.Log($"RangedWeapon: changed damageMultiplier to newDamageMultiplier {newDamageMultiplier}");
        damageMultiplier = newDamageMultiplier;
    }

    private void OnProjectileSpeedMultiplierChangedCB(float newSpeedMultiplier)
    {
        projectileSpeedMultiplier = newSpeedMultiplier;
    }

    void Start()
    {
/*#if UNITY_EDITOR
#else
        projectile = Resources.Load<Projectile>("projectiles/wind ");
#endif*/
        // Same as fixedUpdate
        old_fireRate = fireRate;
        old_fireRateMultiplier = fireRateMultiplier;
        effectiveFireRate = fireRate * fireRateMultiplier;
        shotDelay = 50 / effectiveFireRate;
        counter = shotDelay;
        //Debug.Log(counter);


        OnProjectileSetColor?.Invoke(projectile.color);

        CollectItem.OnItemCollected += collectable =>
        {
            if (collectable.variant == Collectable.Variant.PROJECTILE)
            {
                projectile = collectable.item.GetComponent<Projectile>();
                OnProjectileSetColor?.Invoke(projectile.color);
            }
        };
    }

    private void OnEnable()
    {
        Stats.OnDamageMultiplierChanged += OnDamageMultiplierChangedCB;
        Stats.OnProjectileLifeMultiplierModifierChanged += OnProjectileLifeMultiplierChangedCB;
        Stats.OnProjectileSpeedMultiplierChanged += OnProjectileSpeedMultiplierChangedCB;
        Stats.OnFireRateMultiplierChanged += OnFireRateMultiplierChangedCB;

        GlobalState.OnSceneStart += () =>
        {
            if (GlobalState.projectile != null) projectile = GlobalState.projectile;
        };

        GlobalState.OnSceneEnd += () =>
        {
            GlobalState.projectile = projectile;
        };
    }

    private void OnDisable()
    {
        Stats.OnProjectileLifeMultiplierModifierChanged -= OnProjectileLifeMultiplierChangedCB;
        Stats.OnDamageMultiplierChanged -= OnDamageMultiplierChangedCB;
        Stats.OnProjectileSpeedMultiplierChanged -= OnProjectileSpeedMultiplierChangedCB;
        Stats.OnFireRateMultiplierChanged -= OnFireRateMultiplierChangedCB;
    }

    // Update is called once per frame
    void Update()
    {
        if (old_fireRate != fireRate || old_fireRateMultiplier != fireRateMultiplier)
        {
            old_fireRate = fireRate;
            old_fireRateMultiplier = fireRateMultiplier;
            effectiveFireRate = fireRate * fireRateMultiplier;
            shotDelay = 50 / effectiveFireRate;
            counter = shotDelay;
            //Debug.Log(counter);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && counter >= shotDelay)
        {
            Shoot();
            counter = 0;
        }

        if (counter < shotDelay)
        {
            counter += 1;
        }
    }

    void Shoot()
    {
        //GameObject arrow = Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation);
        //Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
        //rb.velocity = shootingPoint.right * arrowForce;

        var spawnedProjectile = Instantiate(projectile.gameObject, shootingPoint.position, shootingPoint.rotation);
        spawnedProjectile.GetComponent<Projectile>().Setup(
            Projectile.Variant.PLAYER,
            damageMult: damageMultiplier,
            lifetimeMult: projectileLifeMultiplier) ;
        
        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = shootingPoint.right * spawnedProjectile.GetComponent<Projectile>().speed * projectileSpeedMultiplier;
        /*
        spawnedProjectile.GetComponent<Projectile>().lifetime *= projectileLifeMultiplier;
        int totalDamage = (int)(spawnedProjectile.GetComponent<Projectile>().damage * damageMultiplier);
        spawnedProjectile.GetComponent<Projectile>().damage = totalDamage;
        */
        OnFire?.Invoke();

        // FindObjectOfType<AudioManager>().Play("ArrowShot");

        //rb.AddForce(shootingPoint.right * arrowForce, ForceMode2D.Impulse);
    }
}