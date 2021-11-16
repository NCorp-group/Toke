using UnityEngine;

// ReSharper disable InconsistentNaming

public class MovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public float speed = 4f;
    
    
    private Vector2 input_direction = Vector2.zero;
    private bool facing_right = true;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void UpdateAnimation()
    {
        var x = input_direction.x;
        Debug.Log($"x is {x}");


        facing_right = facing_right
            ? !(x < 0)
            : x > 0;
        
        /*
        if (facing_right)
        {
            facing_right = !(x < 0);
        }
        else
        {
            facing_right = x > 0;
        }
        */
        
        Debug.Log($"facing right = {facing_right}");
        
        
        
        anim.SetBool("facing right", facing_right);
        anim.SetFloat("x", x);
        anim.SetFloat("y", input_direction.y);
        anim.SetFloat("magnitude", input_direction.magnitude);
    }
    
    private void MoveCharacter()
    {
        var pos3D = transform.position;
        var pos2D = new Vector2(pos3D.x, pos3D.y);
        rb.MovePosition(pos2D + input_direction * speed * Time.deltaTime);
    }
    
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

    /*
    private void Move()
    {
        var dt = Time.deltaTime;
        var x = Vector3.right * speed * dt * Input.GetAxis("Horizontal");
        var y = Vector3.up * speed * dt * Input.GetAxis("Vertical");
        var heading = Vector3.Normalize(x + y);

        transform.position += x + y;

        UpdateAnimation(heading);
    }
    */

   
    
/*
    var x = dir.x;
        var y = dir.y;
        var idle = Mathf.Approximately(x, 0f) && Mathf.Approximately(y, 0f);
        // anim.SetBool(moving, !idle);
        if (idle)
        {
            anim.SetFloat(LastDirX, lastX);
            anim.SetFloat(LastDirY, lastY);
            anim.SetBool(moving, false);
        }
        else
        {
            lastX = x;
            lastY = y;
            anim.SetBool(moving, true);
        }
        anim.SetFloat(DirX, x);
        anim.SetFloat(DirY, y);
    }
        
*/
}
