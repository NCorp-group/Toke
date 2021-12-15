using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats : MonoBehaviour
{
    public int maxhealth = 100;
    public float movementSpeed = 5;
    public int fireRate = 5;
    public float projectileLifeMultiplier = 1;
    public float damageMultiplier = 1;
    public float luckMultiplier = 1;
    public float projectileSpeedMultiplier = 1;

    public static event Action<float> OnLifeTimeModifierChanged;
    public static event Action<float> OnDamageMultiplierChanged;
    public static event Action<float> OnProjectileSpeedMultiplierChanged;

    private void OnValidate()
    {
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setOnDamageMultiplierChanged(damageMultiplier);
        setOnProjectilespeedMultiplierChanged(projectileSpeedMultiplier);

        GetComponentInParent<PlayerHealthController>().maxHealth = maxhealth;
        GetComponentInChildren<RangedWeapon>().fireRate = fireRate;
        GetComponentInParent<Movement>().movementScalar = movementSpeed;
    }

    void setProjectileLifeMultiplier(float newLifeTimeMultiplier)
    {
        Debug.Log($"About to change newLifeTimeMultiplier to {newLifeTimeMultiplier}");
        projectileLifeMultiplier = newLifeTimeMultiplier;
        OnLifeTimeModifierChanged?.Invoke(projectileLifeMultiplier);
    }

    void setOnDamageMultiplierChanged(float newDmgMultiplier)
    {
        Debug.Log($"About to change damage multiplier to {newDmgMultiplier}");
        damageMultiplier = newDmgMultiplier;
        OnDamageMultiplierChanged?.Invoke(damageMultiplier);
    }

    void setOnProjectilespeedMultiplierChanged(float newSpdMultiplier)
    {
        Debug.Log($"About to change speed multiplier to {newSpdMultiplier}");
        projectileSpeedMultiplier = newSpdMultiplier;
        OnProjectileSpeedMultiplierChanged?.Invoke(projectileSpeedMultiplier);
    }

    // Start is called before the first frame update
    void Start()
    {
        //maxhealth = GetComponentInParent<PlayerHealthController>().maxHealth;
        //fireRate = GetComponentInChildren<RangedWeapon>().fireRate;
        //movementSpeed = GetComponentInParent<Movement>().movementScalar;

        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setOnDamageMultiplierChanged(damageMultiplier);
    }

    // Update is called once per frame
    void Update()
    {

    }
}