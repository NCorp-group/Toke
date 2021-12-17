using System;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    private float old_fireRate;
    [SerializeField] public float fireRate = 5;
    private float shotDelay;
    [SerializeField] private float counter;
    // Start is called before the first frame update

    public static event Action<Color> OnProjectileChanged;
    public static event Action<Color> OnProjectileSetColor;
    public Transform shootingPoint;
    public Projectile projectile;
    private float damageMultiplier = 1;

    //public float projectileSpeed = 5f;
    public static event Action OnFire;

    private float projectileLifeMultiplier = 1;
    private float projectileSpeedMultiplier = 1;

    private void OnFireRateMultiplierChangedCB(float newFireRate)
    {
        fireRate = newFireRate;
    }

    private void OnLifeTimeModifierChangedCB(float newLifeTime) // CB is CallBack
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
        // TODO: don't hard code
        projectile = Resources.Load<Projectile>("projectiles/fire ball");
        old_fireRate = fireRate;
        shotDelay = 50 / fireRate;
        counter = shotDelay;

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
        Stats.OnLifeTimeModifierChanged += OnLifeTimeModifierChangedCB;
        Stats.OnProjectileSpeedMultiplierChanged += OnProjectileSpeedMultiplierChangedCB;
        Stats.OnFireRateChanged += OnFireRateMultiplierChangedCB;

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
        Stats.OnLifeTimeModifierChanged -= OnLifeTimeModifierChangedCB;
        Stats.OnDamageMultiplierChanged -= OnDamageMultiplierChangedCB;
        Stats.OnProjectileSpeedMultiplierChanged -= OnProjectileSpeedMultiplierChangedCB;
        Stats.OnFireRateChanged -= OnFireRateMultiplierChangedCB;
    }

    // Update is called once per frame
    void Update()
    {
        if (old_fireRate != fireRate)
        {
            old_fireRate = fireRate;
            shotDelay = 50 / fireRate;
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
            damageMultiplier: damageMultiplier,
            lifetimeMultiplier: projectileLifeMultiplier) ;
        
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