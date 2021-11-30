using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public Projectile projectile;

    public float speed = 5f;
    public float lifetime = 10f;
    public Transform firepoint;
    private EnemyMovementController ctl;
    private Animator anim;
    private SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        ctl = GetComponent<EnemyMovementController>();
        if (firepoint == null)
        {
            firepoint = GetComponentInChildren<Transform>();
        }

        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("attack"); // projectile fired using animation event
            // StartCoroutine(FireProjectile());
        }
    }

    private void FireProjectile()
    {
        
        var pos = firepoint.position;
        var offset = firepoint.position.x - transform.position.x;
        
        // flip = left
        /*
        if (sr.flipX)
        {
            firepoint.position = new Vector3(pos.x * -1, pos.y, pos.z);
        }
        */
        
        if (!ctl.FacingRight)
        {
            Debug.Log("facing LEFT");
            offset *= -1;
        }
        Debug.Log($"transform.x = {pos.x}");
        
        // mouse position
        Vector3 mousePos = Input.mousePosition;   
        mousePos.z=Camera.main.nearClipPlane;
        Vector3 Worldpos=Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 Worldpos2D = new Vector2(Worldpos.x, Worldpos.y);
        //Worldpos2D is required if you are making a 2D game 
        
        var projectile_spawn_pos = new Vector2(transform.position.x + offset, transform.position.y);
        // var projectile_spawn_pos = new Vector2(transform.position.x, transform.position.y);
        var projectile_direction = (Worldpos2D - projectile_spawn_pos).normalized;
        var angle = Util.GetAngleFromVectorFloat(new Vector3(projectile_direction.x, projectile_direction.y, 0));
        
        var instance = Instantiate(projectile, projectile_spawn_pos, Quaternion.Euler(0, 0, angle));
        // instance.Setup(lifetime, 20);
        
        instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * speed, ForceMode2D.Impulse);
    }
}
