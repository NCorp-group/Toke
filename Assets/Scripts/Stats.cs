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

    public static event Action<float> OnLifeTimeModifierChanged;
    public static event Action<float> OnDamageMultiplierChanged;

    private void OnValidate()
    {
        setProjectileLifeMultiplier(projectileLifeMultiplier);
        setOnDamageMultiplierChanged(damageMultiplier);


        GetComponentInParent<PlayerHealthController>().maxHealth = maxhealth;
        GetComponentInChildren<RangedWeapon>().fireRate = fireRate;
        GetComponentInParent<Movement>().movementScalar = movementSpeed;
    }

    void setProjectileLifeMultiplier(float time)
    {
        //Debug.Log("About to do event");
        projectileLifeMultiplier = time;
        OnLifeTimeModifierChanged?.Invoke(time);
    }

    void setOnDamageMultiplierChanged(float multiplier)
    {
        //Debug.Log($"About to change damage multiplier to {multiplier}");
        damageMultiplier = multiplier;
        OnDamageMultiplierChanged?.Invoke(damageMultiplier);
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