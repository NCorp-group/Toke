using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool FacingRight { get; private set; } = true;

    private Vector2 input_direction = Vector2.zero;


    public float speed = 4f;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
        var x = input_direction.x;

        FacingRight = FacingRight
            ? !(x < 0)
            : x > 0;
        
        sr.flipX = !FacingRight;
        
        anim.SetFloat("x", x);
        anim.SetFloat("y", input_direction.y);
        anim.SetFloat("speed", input_direction.magnitude);
    }
}
