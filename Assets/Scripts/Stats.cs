using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    private const string PENNINGARS = "penningars";
    private const string CURRENT_HEALTH = "current_health";
    private const string MAX_HEALTH = "max_health";
    private const string MOVEMENT_SPEED = "movement_speed";
    private const string LUCK_MULTIPLIER = "luck_multiplier";
    private const string FIRE_RATE = "fire_rate";
    private const string DAMAGE_MULTIPLIER = "damage_multipler";
    private const string PROJECTILE_LIFE_MULTIPLIER = "projectile_life_multiplier";
    private const string PROJECTILE_SPEED_MULTIPLIER = "projectile_speed_multiplier";

    public int penningar = 0;
    public int maxHealth = 100;
    public float currentHealth = 120;
    public float movementSpeed = 5;
    public float luckMultiplier = 1;
    public float fireRateMultiplier = 5;
    public float damageMultiplier = 1;
    public float projectileLifeMultiplier = 1;
    public float projectileSpeedMultiplier = 1;
    
    public static event Action<float, int> OnPlayerHealthChange;
    public static event Action OnPlayerDie;
    public static event Action OnPlayerTakeDamage;
    
    public bool alive = true;

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
        StatsFromPlayerPrefs();
        
        setMaxHealth(maxHealth);
        setCurrentHealth(currentHealth);
        setMovementSpeedMultiplier(movementSpeed);
        setLuckMultiplier(luckMultiplier);
        setFireRateMultiplier(fireRateMultiplier);
        setDamageMultiplier(damageMultiplier);
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setProjectilespeedMultiplier(projectileSpeedMultiplier);
    }

    //#endif
    /////////////////////////////For consumables////////////////////////////////
    //
    public void addPenningarAmount(int addPeningar)
    {
        penningar += addPeningar;
        OnPenningarAmountChanged?.Invoke(penningar);
        PenningarToPlayerPrefs();
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
    public void addCurrentHealth(float addCurrentHealth)
    {
        //Debug.Log($"About to add {addCurrentHealth} to currentHealth");
        if (addCurrentHealth != 0)
        {
            var newCurrentHealth = currentHealth + addCurrentHealth;
            currentHealth = newCurrentHealth > maxHealth ? maxHealth : newCurrentHealth < 0 ? 0 : newCurrentHealth;
            //Debug.Log($"New current health = {currentHealth}");
            OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
            CurrentHealthToPlayerPrefs();
        }
    }
    public void addMaxHealth(int addMaxHealth)
    {
        //Debug.Log($"About to add {addMaxHealth} to max HP");
        if (addMaxHealth != 0)
        {
            maxHealth += addMaxHealth;
            addCurrentHealth(addMaxHealth);
            //Debug.Log($"New max health = {maxHealth}");
            //Debug.Log($"New current health = {currentHealth}");
            OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
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
        //Debug.Log($"About to speed up fire rate by {addFireRate}");
        if (addFireRate != 0)
        {
            fireRateMultiplier += addFireRate;
            OnFireRateMultiplierChanged?.Invoke(fireRateMultiplier);
            FireRateMultiplierToPlayerPrefs();
        }
    }
    public void addLuckMultiplier(float addLuck)
    {
        //Debug.Log($"About to increment luck by {addLuck}");
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
        PenningarToPlayerPrefs();

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
    void setCurrentHealth(float newCurrentHealth)
    {
        //Debug.Log($"About to set {newCurrentHealth} to current health");
        currentHealth = newCurrentHealth > maxHealth ? maxHealth : newCurrentHealth < 0 ? 0 : newCurrentHealth;
        //Debug.Log($"New current health = {currentHealth}");
        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
        CurrentHealthToPlayerPrefs();
    }
    void setMaxHealth(int totalMaxHealth)
    {
        //Debug.Log($"About to set {totalMaxHealth} to max HP");
        maxHealth = totalMaxHealth;
        //Debug.Log($"New max health = {maxHealth}");
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
    }

    // Start is called before the first frame update
    private void Start()
    {
        StatsFromPlayerPrefs();
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setDamageMultiplier(damageMultiplier);
        setProjectilespeedMultiplier(projectileSpeedMultiplier);
        setCurrentHealth(currentHealth);
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
        //Debug.Log($"PP: projectile life multiplier = {PlayerPrefs.GetFloat(PROJECTILE_LIFE_MULTIPLIER, -1)}");
        
        setDamageMultiplier(PlayerPrefs.GetFloat(DAMAGE_MULTIPLIER, damageMultiplier));
        //Debug.Log($"PP: damage multiplier = {PlayerPrefs.GetFloat(DAMAGE_MULTIPLIER, -1)}");
        
        setProjectilespeedMultiplier(PlayerPrefs.GetFloat(PROJECTILE_SPEED_MULTIPLIER, projectileSpeedMultiplier));
        //Debug.Log($"PP: prjectile speed multiplier = {PlayerPrefs.GetFloat(PROJECTILE_SPEED_MULTIPLIER, -1)}");
        
        setMaxHealth(PlayerPrefs.GetInt(MAX_HEALTH, maxHealth));
        //Debug.Log($"PP: max health = {PlayerPrefs.GetInt(MAX_HEALTH, -1)}");
        
        setCurrentHealth(PlayerPrefs.GetFloat(CURRENT_HEALTH, currentHealth));
        //Debug.Log($"PP: current health = {PlayerPrefs.GetFloat(CURRENT_HEALTH, -1)}");
        
        setMovementSpeedMultiplier(PlayerPrefs.GetFloat(MOVEMENT_SPEED, movementSpeed));
        //Debug.Log($"PP: movement speed = {PlayerPrefs.GetFloat(MOVEMENT_SPEED, -1)}");
        
        setFireRateMultiplier(PlayerPrefs.GetFloat(FIRE_RATE, fireRateMultiplier));
        //Debug.Log($"PP: fire rate = {PlayerPrefs.GetFloat(FIRE_RATE, -1)}");
        
        setLuckMultiplier(PlayerPrefs.GetFloat(LUCK_MULTIPLIER, luckMultiplier));
        //Debug.Log($"PP: luck multiplier = {PlayerPrefs.GetFloat(LUCK_MULTIPLIER, -1)}");
    }

    private void StatsToPlayerPrefs()
    {
        PlayerPrefs.SetFloat(PROJECTILE_LIFE_MULTIPLIER, projectileLifeMultiplier);
        PlayerPrefs.SetFloat(DAMAGE_MULTIPLIER, damageMultiplier);
        PlayerPrefs.SetFloat(PROJECTILE_SPEED_MULTIPLIER, projectileSpeedMultiplier);
        PlayerPrefs.SetFloat(CURRENT_HEALTH,currentHealth);
        PlayerPrefs.SetInt(MAX_HEALTH, maxHealth);
        PlayerPrefs.SetFloat(MOVEMENT_SPEED, movementSpeed);
        PlayerPrefs.SetFloat(FIRE_RATE, fireRateMultiplier);
        PlayerPrefs.SetFloat(LUCK_MULTIPLIER, luckMultiplier);
    }

    private void PenningarToPlayerPrefs()
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

    private void CurrentHealthToPlayerPrefs()
    {
        //Debug.Log($"PP BEFORE: current health = {PlayerPrefs.GetFloat(CURRENT_HEALTH, -1)}");
        PlayerPrefs.SetFloat(CURRENT_HEALTH, currentHealth);
        //Debug.Log($"PP AFTER: current health = {PlayerPrefs.GetFloat(CURRENT_HEALTH, 0)}");
    }

    private void MaxHealthToPlayerPrefs()
    {
        //Debug.Log($"PP BEFORE: max health = {PlayerPrefs.GetInt(MAX_HEALTH, -1)}");
        //Debug.Log($"ACTUAL BEFORE: max health = {maxHealth}");
        PlayerPrefs.SetInt(MAX_HEALTH, maxHealth);
        //Debug.Log($"ACTUAL BEFORE: max health = {maxHealth}");
        //Debug.Log($"PP AFTER: max health = {PlayerPrefs.GetInt(MAX_HEALTH, -1)}");
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
    
    public void TakeDamage(float damage)
    {
        if (!alive) return;
        /*currentHealth -= damage;
        PlayerPrefs.SetFloat(CURRENT_HEALTH, currentHealth);*/
        addCurrentHealth(-damage);

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        CurrentHealthToPlayerPrefs();
        //OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);

        if (currentHealth > 0)
        {
            OnPlayerTakeDamage?.Invoke();
        }
        else
        {
            OnPlayerDie?.Invoke();
            alive = false;
        }
    }
}