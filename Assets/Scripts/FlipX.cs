using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipX : MonoBehaviour
{
    private Rigidbody2D rbody;
    private SpriteRenderer sr;
    
    // Start is called before the first frame update
    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        var x = rbody.velocity.x;
        Debug.Log($"hello anyone? x = {x}");
        if (x >= 0.01f)
        {
            sr.flipX = false;
            Debug.Log("facing right");
        }
        else if (x <= -0.01f)
        {
            sr.flipX = true;
            Debug.Log("facing left");
        }
    }
}
