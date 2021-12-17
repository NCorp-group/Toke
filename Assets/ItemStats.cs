using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemStats : MonoBehaviour
{
    // Stats:
    public int maxHealth = 0;
    public float movementSpeed = 0;
    public float luckMultiplier = 0;
    public float fireRate = 0;
    public float damageMultiplier = 0;
    public float projectileLifeMultiplier = 0;
    public float projectileSpeedMultiplier = 0;


    bool dropped = true;
    int price = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = $"{price} P";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
