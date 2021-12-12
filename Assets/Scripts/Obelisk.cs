using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    public Projectile projectile;
    
    public Transform target;
    public float firerate = 2f;
    public float speed = 5f;
    public float distance = 20f;

    private Animator anim;
    private SpriteRenderer sr;


    public enum TargetType
    {
        PLAYER,
        ENEMY,
    };

    
    [Header("NOT USED YET")]
    public List<TargetType> includeTargetsOfType = new();

    private float t = 0f;
    private RaycastHit2D response;
    private Transform firepoint;

    private HashSet<TargetType> _includedTargets = new();

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        firepoint = transform.Find("Firepoint");

        _includedTargets = includeTargetsOfType.ToHashSet();
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= firerate)
        {
            t = 0;
            // TODO: use layer mask
            var direction = (target.position - firepoint.position).normalized;
            //Debug.Log($"casting ray in direction {direction}");
            var mask = LayerMask.GetMask("Props", "Walls", "Player");
            var hit = Physics2D.Raycast(firepoint.position, direction, distance, mask);
            //Debug.Log("I hit something");
            if (hit.collider == null || hit.rigidbody.gameObject.CompareTag("Player"))
            {
                //Debug.Log("firing");
                Fire(hit);
                //Debug.DrawLine(firepoint.position, hit.point, Color.red, 2f);
            }
        }
    }

    // called by animation event
    private void Fire(RaycastHit2D hit)
    {
        //Debug.Log("I am in the fire");
        var direction = (target.position - firepoint.position ).normalized;
        var angle = Utilities.GetAngleFromVectorFloat(direction);
        var p = Instantiate(projectile, firepoint.position, Quaternion.Euler(0, 0, angle));
        p.GetComponent<Rigidbody2D>()?.AddForce(direction * speed, ForceMode2D.Impulse);
    }
}
