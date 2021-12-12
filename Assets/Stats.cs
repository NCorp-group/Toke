using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stats : MonoBehaviour
{
    public int maxhealth = 100;
    public int fireRate = 5;
    public float projectileLifeMultiplier = 1;
    public float movementSpeed = 5;
    public float damageMultiplier = 1;
    public float luckMultiplier = 1;

    public static event Action<float> OnLifeTimeModifierChanged;

    private void OnValidate()
    {
        setProjectileLifeTimeMultiplier(projectileLifeMultiplier);
    }

    void setProjectileLifeTimeMultiplier(float time)
    {
        //Debug.Log("About to do event");
        projectileLifeMultiplier = time;
        OnLifeTimeModifierChanged?.Invoke(time);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxhealth = GetComponentInParent<PlayerHealthController>().maxHealth;
        fireRate = GetComponentInChildren<RangedWeapon>().fireRate;
        movementSpeed = GetComponentInParent<Movement>().movementScalar;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInParent<PlayerHealthController>().maxHealth = maxhealth;
        GetComponentInChildren<RangedWeapon>().fireRate = fireRate;
        GetComponentInParent<Movement>().movementScalar = movementSpeed;
    }
}