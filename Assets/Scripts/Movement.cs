using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Animator animator;

    public void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical",   movement.y);
        animator.SetFloat("Magnitude",  movement.magnitude);


        if (movement.magnitude > 1.0f)
        {
            movement.Normalize();
        }
        transform.position = transform.position + movement * Time.fixedDeltaTime;
        FindObjectOfType<AudioManager>().Play("TestSound");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Move();
    }
}
