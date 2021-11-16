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
    public float movementScalar = 16;
    private int x;
    private int y;

    public void Move()
    {/*
        x = Input.GetAxis("Horizontal") switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0,
        };

        y = Input.GetAxis("Vertical") switch
        {
            > 0 => 1,
            < 0 => -1,
            _ => 0,
        };


        */
        var w = Input.GetKey(KeyCode.W);
        var a = Input.GetKey(KeyCode.A);
        var s = Input.GetKey(KeyCode.S);
        var d = Input.GetKey(KeyCode.D);

        x = (a ? -1 : 0) + (d ? 1 : 0);
        y = (s ? -1 : 0) + (w ? 1 : 0);

        Vector2 movement = new Vector2(x, y);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude",  movement.magnitude);

        if (movement.magnitude > 1.0f)
        {
            movement.Normalize();
        }
        transform.position = transform.position + (Vector3) movement * Time.fixedDeltaTime * movementScalar;

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
/*
    private Rigidbody2D rb;

    private Vector2 input_direction = Vector2.zero;

    public float speed = 4f;
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        var hrz = Input.GetAxis("Horizontal");
        var vtc = Input.GetAxis("Vertical");
        input_direction = new Vector2(hrz, vtc);
        input_direction.Normalize();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        UpdateAnimation();
    }

    private void MoveCharacter()
    {
        var pos3D = transform.position;
        var pos2D = new Vector2(pos3D.x, pos3D.y);
        rb.MovePosition(pos2D + input_direction * speed * Time.deltaTime);
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("x", input_direction.x);
        animator.SetFloat("y", input_direction.y);
        animator.SetFloat("Magnitude", input_direction.magnitude);
    }
}*/