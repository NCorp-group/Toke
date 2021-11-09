using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    

    public Rigidbody2D rb;
    public Animator animator;

    public float speed = 5.0f;
    
    public GameObject arrowPrefab;
    
    private Vector2 movement;
    
// Start is called before the first frame update
private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }


private void UpdateAnimation(Vector2 movement)
{
    animator.SetFloat("Horizontal", movement.x);
    animator.SetFloat("Vertical",   movement.y);
    animator.SetFloat("Magnitude",  movement.magnitude);
}

    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        if (movement.magnitude > 1.0f)
        {
            movement.Normalize();
        }
        transform.position += movement * speed * Time.deltaTime;
    }

    private void MoveCharacter(Vector2 direction)
    {
        var pos = transform.position;
        var pos2D = new Vector2(pos.x, pos.y);
        rb.MovePosition(pos2D + (direction * speed * Time.deltaTime));
    }

    // Update is called once per frame
    private void Update()
    {
        var hrz = Input.GetAxis("Horizontal");
        var vtc = Input.GetAxis("Vertical");
        movement = new Vector2(hrz, vtc);
    }

    private void FixedUpdate()
    {
        MoveCharacter(movement);
        UpdateAnimation(movement);
    }
}
