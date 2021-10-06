using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    public float lifeTime = 1.5f;
    void  Awake ()
    {
        Destroy(gameObject, lifeTime);
    }

    public GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision){
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        //Destroy(effect, 0.30f); // Destroy object after 5 seconds of hitting something
        //Destroy(gameObject);
    }
}
