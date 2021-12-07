using System;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{
    private int old_fireRate;
    [SerializeField] private int fireRate = 5;
    private int shotDelay;
    [SerializeField] private int counter;
    // Start is called before the first frame update

    public static event Action<Color> OnProjectileChanged;
    public static event Action<Color> OnProjectileSetColor;
    public Transform shootingPoint;
    public Projectile projectile;
    
    public float arrowForce = 5f;
    public static event Action OnFire;
    
    void Start()
    {
        // TODO: don't hard code
        projectile = Resources.Load<Projectile>("projectiles/wind arc");
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
        GlobalState.OnSceneStart += () =>
        {
            if (GlobalState.projectile != null) projectile = GlobalState.projectile;
        };

        GlobalState.OnSceneEnd += () =>
        {
            GlobalState.projectile = projectile;
        };
    }


    // Update is called once per frame
    void Update()
    {
        if (old_fireRate != fireRate)
        {
            old_fireRate = fireRate;
            shotDelay = 50 / fireRate;
            counter = shotDelay;
            Debug.Log(counter);
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
        spawnedProjectile.GetComponent<Rigidbody2D>().velocity = shootingPoint.right * arrowForce;
        
        OnFire?.Invoke();

        // FindObjectOfType<AudioManager>().Play("ArrowShot");

        //rb.AddForce(shootingPoint.right * arrowForce, ForceMode2D.Impulse);
    }
}