using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    private const string PENNINGARS = "penningars";
    private const string MAX_HEALTH = "max_health";
    private const string MOVEMENT_SPEED = "movement_speed";
    private const string LUCK_MULTIPLIER = "luck_multiplier";
    private const string FIRE_RATE = "fire_rate";
    private const string DAMAGE_MULTIPLIER = "damage_multipler";
    private const string PROJECTILE_LIFE_MULTIPLIER = "projectile_life_multiplier";
    private const string PROJECTILE_SPEED_MULTIPLIER = "projectile_speed_multiplier";

    public int penningar = 0;
    public int maxHealth = 100;
    public float movementSpeed = 5;
    public float luckMultiplier = 1;
    public float fireRateMultiplier = 5;
    public float damageMultiplier = 1;
    public float projectileLifeMultiplier = 1;
    public float projectileSpeedMultiplier = 1;

    public static event Action<int> OnPenningarAmountChanged;
    public static event Action<float> OnProjectileLifeMultiplierModifierChanged;
    public static event Action<float> OnDamageMultiplierChanged;
    public static event Action<float> OnLuckMultiplierChanged;
    public static event Action<float> OnProjectileSpeedMultiplierChanged;
    public static event Action<int> OnMaxHealthChanged;
    public static event Action<float> OnMovementSpeedMultiplierChanged;
    public static event Action<float> OnFireRateMultiplierChanged;

    //#if UNITY_EDITOR
    private void OnValidate()
    {
        //Debug.Log("!! OnValidate");
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setDamageMultiplier(damageMultiplier);
        setProjectilespeedMultiplier(projectileSpeedMultiplier);
        setMaxHealth(maxHealth);
        setMovementSpeedMultiplier(movementSpeed);
        setFireRateMultiplier(fireRateMultiplier);

        StatsToPlayerPrefs();
        //GetComponentInParent<PlayerHealthController>().maxHealth = maxHealth;
    }

    //#endif
    /////////////////////////////For consumables////////////////////////////////
    //
    public void addPenningarAmount(int addPeningar)
    {
        penningar += addPeningar;
        OnPenningarAmountChanged?.Invoke(penningar);
        PenningarPlayerPrefs();
    }
    public void addProjectileLifeMultiplier(float newLifeTimeMultiplier)
    {
        //Debug.Log($"About to add {newLifeTimeMultiplier} to newLifeTimeMultiplier");
        if (newLifeTimeMultiplier != 0)
        {
            projectileLifeMultiplier += newLifeTimeMultiplier;
            OnProjectileLifeMultiplierModifierChanged?.Invoke(projectileLifeMultiplier);
            ProjectileLifeMultiplierToPlayerPrefs();
        }
    }
    public void addDamageMultiplier(float newDmgMultiplier)
    {
        //Debug.Log($"About to add damage multiplier by {newDmgMultiplier}");
        if (newDmgMultiplier != 0)
        {
            damageMultiplier += newDmgMultiplier;
            OnDamageMultiplierChanged?.Invoke(damageMultiplier);
            DamageMultiplierToPlayerPrefs();
        }
    }
    public void addProjectilespeedMultiplier(float newSpdMultiplier)
    {
        //Debug.Log($"About to add {newSpdMultiplier} to the speed multiplier ");
        if (newSpdMultiplier != 0)
        {
            projectileSpeedMultiplier += newSpdMultiplier;
            OnProjectileSpeedMultiplierChanged?.Invoke(projectileSpeedMultiplier);
            ProjectileSpeedMultiplierToPlayerPrefs();
        }
        //Debug.Log($"In Add: the projectile speed multiplier is now: {projectileSpeedMultiplier}");
    }
    public void addMaxHealth(int addMaxHealth)
    {
        //Debug.Log($"About to add {addMaxHealth} to max HP");
        if (addMaxHealth != 0)
        {
            maxHealth += addMaxHealth;
            OnMaxHealthChanged?.Invoke(maxHealth);
            MaxHealthToPlayerPrefs();
        }
    }
    public void addMovementSpeedScalar(float addMovementSpeed)
    {
        //Debug.Log($"About to speed up movement by {addMovementSpeed}");
        if (addMovementSpeed != 0)
        {
            movementSpeed += addMovementSpeed;
            OnMovementSpeedMultiplierChanged?.Invoke(movementSpeed);
            MovementSpeedToPlayerPrefs();
        }
    }
    public void addFireRateScalar(float addFireRate)
    {
        //Debug.Log($"Abou to speed up fire rate by {addFireRate}");
        if (addFireRate != 0)
        {
            fireRateMultiplier += addFireRate;
            OnFireRateMultiplierChanged?.Invoke(fireRateMultiplier);
            FireRateMultiplierToPlayerPrefs();
        }
    }
    public void addLuckMultiplier(float addLuck)
    {
        //Debug.Log($"Abou to speed up fire rate by {addLuck}");
        if (addLuck != 0)
        {
            luckMultiplier += addLuck;
            OnLuckMultiplierChanged?.Invoke(luckMultiplier);
            LuckMultiplierToPlayerPrefs();
        }
    }

    ///////////////////////////////For debug UI/////////////////////////////////
    public void setPeningarAmount(int setPeningar)
    {
        penningar = setPeningar;
        OnPenningarAmountChanged?.Invoke(penningar);
        PenningarPlayerPrefs();

    }
    void setProjectileLifeMultiplier(float newLifeTimeMultiplier)
    {
        //Debug.Log($"About to change newLifeTimeMultiplier to {newLifeTimeMultiplier}");
        projectileLifeMultiplier = newLifeTimeMultiplier;
        OnProjectileLifeMultiplierModifierChanged?.Invoke(projectileLifeMultiplier);
        ProjectileLifeMultiplierToPlayerPrefs();
    }
    void setDamageMultiplier(float newDmgMultiplier)
    {
        //Debug.Log($"About to change damage multiplier to {newDmgMultiplier}");
        damageMultiplier = newDmgMultiplier;
        OnDamageMultiplierChanged?.Invoke(damageMultiplier);
        DamageMultiplierToPlayerPrefs();
    }
    void setProjectilespeedMultiplier(float newSpdMultiplier)
    {
        //Debug.Log($"About to change speed multiplier to {newSpdMultiplier}");
        projectileSpeedMultiplier = newSpdMultiplier;
        OnProjectileSpeedMultiplierChanged?.Invoke(projectileSpeedMultiplier);
        ProjectileSpeedMultiplierToPlayerPrefs();
    }
    void setMaxHealth(int totalMaxHealth)
    {
        //Debug.Log($"About to add {totalMaxHealth} to max HP");
        maxHealth = totalMaxHealth;
        OnMaxHealthChanged?.Invoke(maxHealth);
        MaxHealthToPlayerPrefs();
    }
    void setMovementSpeedMultiplier(float newMovementSpeed)
    {
        //Debug.Log($"About to speed up movement by {newMovementSpeed}");
        movementSpeed = newMovementSpeed;
        OnMovementSpeedMultiplierChanged?.Invoke(movementSpeed);
        MovementSpeedToPlayerPrefs();
    }
    void setFireRateMultiplier(float newFireRate)
    {
        //Debug.Log($"About to speed up fire rate by {newFireRate}");
        fireRateMultiplier = newFireRate;
        OnFireRateMultiplierChanged?.Invoke(fireRateMultiplier);
        FireRateMultiplierToPlayerPrefs();
    }
    void setLuckMultiplier(float setLuck)
    {
        //Debug.Log($"About to increment luck by {setLuck}");
        luckMultiplier = setLuck;
        OnLuckMultiplierChanged?.Invoke(luckMultiplier);
        LuckMultiplierToPlayerPrefs();
    }
    ////////////////////////////////////////////////////////////////////////////

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == RoomManager.ROOM_ENTRY)
        {
            PlayerPrefs.DeleteAll();
        }
        StatsFromPlayerPrefs();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setDamageMultiplier(damageMultiplier);
        setProjectilespeedMultiplier(projectileSpeedMultiplier);
        setMaxHealth(maxHealth);
        setMovementSpeedMultiplier(movementSpeed);
        setFireRateMultiplier(fireRateMultiplier);
        setLuckMultiplier(luckMultiplier);
    }

    private void OnDisable()
    {
    }

    private void StatsFromPlayerPrefs()
    {
        setPeningarAmount(PlayerPrefs.GetInt(PENNINGARS, penningar));
        setProjectileLifeMultiplier(PlayerPrefs.GetFloat(PROJECTILE_LIFE_MULTIPLIER, projectileLifeMultiplier));
        setDamageMultiplier(PlayerPrefs.GetFloat(DAMAGE_MULTIPLIER, damageMultiplier));
        setProjectilespeedMultiplier(PlayerPrefs.GetFloat(PROJECTILE_SPEED_MULTIPLIER, projectileSpeedMultiplier));
        //Debug.Log("FROM PP: Projectile Speed: " + projectileSpeedMultiplier);
        setMaxHealth(PlayerPrefs.GetInt(MAX_HEALTH, maxHealth));
        setMovementSpeedMultiplier(PlayerPrefs.GetFloat(MOVEMENT_SPEED, movementSpeed));
        setFireRateMultiplier(PlayerPrefs.GetFloat(FIRE_RATE, fireRateMultiplier));
        setLuckMultiplier(PlayerPrefs.GetFloat(LUCK_MULTIPLIER, luckMultiplier));
    }

    private void StatsToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(PROJECTILE_LIFE_MULTIPLIER, projectileLifeMultiplier);
        PlayerPrefs.SetFloat(DAMAGE_MULTIPLIER, damageMultiplier);
        //Debug.Log("TO PP: Projectile Speed: " + projectileSpeedMultiplier);
        PlayerPrefs.SetFloat(PROJECTILE_SPEED_MULTIPLIER, projectileSpeedMultiplier);
        PlayerPrefs.SetInt(MAX_HEALTH, maxHealth);
        PlayerPrefs.SetFloat(MOVEMENT_SPEED, movementSpeed);
        PlayerPrefs.SetFloat(FIRE_RATE, fireRateMultiplier);
        PlayerPrefs.SetFloat(LUCK_MULTIPLIER, luckMultiplier);
    }

    private void PenningarPlayerPrefs()
    {
        PlayerPrefs.SetInt(PENNINGARS, penningar);
    }
    private void ProjectileLifeMultiplierToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(PROJECTILE_LIFE_MULTIPLIER, projectileLifeMultiplier);
    }

    private void DamageMultiplierToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(DAMAGE_MULTIPLIER, damageMultiplier);
    }

    private void ProjectileSpeedMultiplierToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(PROJECTILE_SPEED_MULTIPLIER, projectileSpeedMultiplier);
    }

    private void MaxHealthToPlayerPrefs()
    {
        PlayerPrefs.SetInt(MAX_HEALTH, maxHealth);
    }

    private void MovementSpeedToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(MOVEMENT_SPEED, movementSpeed);
    }

    private void FireRateMultiplierToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(FIRE_RATE, fireRateMultiplier);
    }

    private void LuckMultiplierToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(LUCK_MULTIPLIER, luckMultiplier);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("FROM PP: Projectile Speed Multiplier: " + PlayerPrefs.GetFloat(PROJECTILE_SPEED_MULTIPLIER, 0));
    }
}