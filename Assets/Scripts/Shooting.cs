using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int old_fireRate;
    [SerializeField] private int fireRate = 5;
    private int shotDelay;
    [SerializeField] private int counter;
    // Start is called before the first frame update

    public static event Action OnFire;
    
    void Start()
    {
        old_fireRate = fireRate;
        shotDelay = 50 / fireRate;
        counter = shotDelay;
        //Debug.Log("Hello");
    }

    public Transform shootingPoint;
    public GameObject arrowPrefab;

    public float arrowForce = 5f;

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
        GameObject arrow = Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        rb.velocity = shootingPoint.right * arrowForce;
        OnFire?.Invoke();

        //rb.AddForce(shootingPoint.right * arrowForce, ForceMode2D.Impulse);
    }
}