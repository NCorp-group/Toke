using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform shootingPoint;
    public GameObject arrowPrefab;

    public float arrowForce = 5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")){
            Shoot();
        }
    }

    void Shoot(){
        GameObject arrow = Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        rb.AddForce(shootingPoint.right * arrowForce, ForceMode2D.Impulse);
    }
}
