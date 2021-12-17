using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats : MonoBehaviour
{
    public int maxHealth = 100;
    public float movementSpeed = 5;
    public float luckMultiplier = 1;
    public float fireRate = 5;
    public float damageMultiplier = 1;
    public float projectileLifeMultiplier = 1;
    public float projectileSpeedMultiplier = 1;

    public static event Action<float> OnLifeTimeModifierChanged;
    public static event Action<float> OnDamageMultiplierChanged;
    public static event Action<float> OnLuckMultiplierChanged;
    public static event Action<float> OnProjectileSpeedMultiplierChanged;
    public static event Action<float> OnMaxHealthChanged;               // TODO: Needs a listener on the other end. Then check if it works when Jens' UI is ready and can show the max HP
    public static event Action<float> OnMovementSpeedMultiplierChanged;
    public static event Action<float> OnFireRateChanged;

    //#if UNITY_EDITOR
    private void OnValidate()
    {
        //Debug.Log("!! OnValidate");
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setOnDamageMultiplierChanged(damageMultiplier);
        setOnProjectilespeedMultiplierChanged(projectileSpeedMultiplier);
        setMaxHealthChanged(maxHealth);
        setMovementSpeedMultiplierChanged(movementSpeed);
        setFireRateScalarChanged(fireRate);
        //GetComponentInParent<PlayerHealthController>().maxHealth = maxHealth;
    }
    //#endif
    /////////////////////////////For consumables////////////////////////////////
    // 
    public void addProjectileLifeMultiplier(float newLifeTimeMultiplier)
    {
        //Debug.Log($"About to add {newLifeTimeMultiplier} to newLifeTimeMultiplier");
        if (newLifeTimeMultiplier != 0)
        {
            projectileLifeMultiplier += newLifeTimeMultiplier;
            OnLifeTimeModifierChanged?.Invoke(projectileLifeMultiplier);
        }
    }
    public void addOnDamageMultiplierChanged(float newDmgMultiplier)
    {
        //Debug.Log($"About to add damage multiplier by {newDmgMultiplier}");
        if (newDmgMultiplier != 0)
        {
            damageMultiplier += newDmgMultiplier;
            OnDamageMultiplierChanged?.Invoke(damageMultiplier);
        }
    }
    public void addOnProjectilespeedMultiplierChanged(float newSpdMultiplier)
    {
        //Debug.Log($"About to add {newSpdMultiplier} to the speed multiplier ");
        if (newSpdMultiplier != 0)
        {
            projectileSpeedMultiplier += newSpdMultiplier;
            OnProjectileSpeedMultiplierChanged?.Invoke(projectileSpeedMultiplier);
        }
    }
    public void addMaxHealthChanged(int addMaxHealth)
    {
        //Debug.Log($"About to add {addMaxHealth} to max HP");
        if (addMaxHealth != 0)
        { 
            maxHealth += addMaxHealth;
            OnMaxHealthChanged?.Invoke(maxHealth);
        }
    }
    public void addMovementSpeedScalarChanged(float addMovementSpeed)
    {
        //Debug.Log($"About to speed up movement by {addMovementSpeed}");
        if (addMovementSpeed != 0)
        {
            movementSpeed += addMovementSpeed;
            OnMovementSpeedMultiplierChanged?.Invoke(movementSpeed);
        }
    }
    public void addFireRateScalarChanged(float addFireRate)
    {
        //Debug.Log($"Abou to speed up fire rate by {addFireRate}");
        if (addFireRate != 0)
        {
            fireRate += addFireRate;
            OnFireRateChanged?.Invoke(fireRate);
        }
    }
    public void addLuckMultiplierChanged(float addLuck)
    {
        //Debug.Log($"Abou to speed up fire rate by {addLuck}");
        if (addLuck != 0)
        {
            luckMultiplier += addLuck;
            OnLuckMultiplierChanged?.Invoke(luckMultiplier);
        }
    }
    ///////////////////////////////For debug UI/////////////////////////////////
    void setProjectileLifeMultiplier(float newLifeTimeMultiplier)
    {
        //Debug.Log($"About to change newLifeTimeMultiplier to {newLifeTimeMultiplier}");
        projectileLifeMultiplier = newLifeTimeMultiplier;
        OnLifeTimeModifierChanged?.Invoke(projectileLifeMultiplier);
    }
    void setOnDamageMultiplierChanged(float newDmgMultiplier)
    {
        //Debug.Log($"About to change damage multiplier to {newDmgMultiplier}");
        damageMultiplier = newDmgMultiplier;
        OnDamageMultiplierChanged?.Invoke(damageMultiplier);
    }
    void setOnProjectilespeedMultiplierChanged(float newSpdMultiplier)
    {
        //Debug.Log($"About to change speed multiplier to {newSpdMultiplier}");
        projectileSpeedMultiplier = newSpdMultiplier;
        OnProjectileSpeedMultiplierChanged?.Invoke(projectileSpeedMultiplier);
    }
    void setMaxHealthChanged(int totalMaxHealth)
    {
        //Debug.Log($"About to add {totalMaxHealth} to max HP");
        maxHealth = totalMaxHealth;
        OnMaxHealthChanged?.Invoke(maxHealth);
    }
    void setMovementSpeedMultiplierChanged(float newMovementSpeed)
    {
        //Debug.Log($"About to speed up movement by {newMovementSpeed}");
        movementSpeed = newMovementSpeed;
        OnMovementSpeedMultiplierChanged?.Invoke(movementSpeed);
    }
    void setFireRateScalarChanged(float newFireRate)
    {
        //Debug.Log($"Abou to speed up fire rate by {newFireRate}");
        fireRate = newFireRate;
        OnFireRateChanged?.Invoke(fireRate);
    }
    void setLuckMultiplierChanged(float setLuck)
    {
        Debug.Log($"Abou to speed up fire rate by {setLuck}");
        luckMultiplier = setLuck;
        OnLuckMultiplierChanged?.Invoke(luckMultiplier);
    }
    ////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setOnDamageMultiplierChanged(damageMultiplier);
        setOnProjectilespeedMultiplierChanged(projectileSpeedMultiplier);
        setMaxHealthChanged(maxHealth);
        setMovementSpeedMultiplierChanged(movementSpeed);
        setFireRateScalarChanged(fireRate);
    }

    // Update is called once per frame
    void Update()
    {

    }
}