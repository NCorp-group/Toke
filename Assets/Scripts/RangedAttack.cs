using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class RangedAttack : MonoBehaviour
{
    public static event Action<Enemy.EnemyType> OnEnemyRangedAttack;

    
    private Enemy.EnemyType _enemyType;
    
    public Projectile projectile;
    private CircleCollider2D _circleCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;
    
    public float speed = 5f;
    public float lifetime = 10f;

    [Header("This requires a child object with tag = \"FirePoint\"")]
    [SerializeField] private bool useFirePoint = true;

    [Header("If true then responsibility of calling \"Fire()\" is delegated to an animation event on the object, with trigger named \"attack\"")]
    [SerializeField] private bool useAnimationEventToTriggerProjectileFire = false;

    [SerializeField] private Projectile.Variant projectileVariant = Projectile.Variant.ENEMY;

    [Range(0f, 1.0f)]
    [SerializeField] private float projectilePadding;
    
    private Transform _firepoint;
    private Animator _animator;
    private SpriteRenderer _sr;

    private Transform _child_firepoint;
    private Func<Transform> GetFirePoint;

    private Func<float> _get_width_of_collider;

    // Start is called before the first frame update
    private void Start()
    {
        _enemyType = GetComponent<Enemy>().type;

        _animator = GetComponent<Animator>();
        if (useFirePoint)
        {
            _child_firepoint = GetComponentsInChildren<Transform>().FirstOrDefault(c => c.CompareTag("FirePoint"));
            Assert.IsNotNull(_child_firepoint);
            GetFirePoint = () => _child_firepoint;
        }
        else
        {
            GetFirePoint = () => transform;
        }

        if (useAnimationEventToTriggerProjectileFire)
        {
            // This trigger, should call FireProjectile() using a animation event.
            _Fire = () => _animator.SetTrigger("attack");
        }
        else
        {
            _Fire = () => FireProjectile();
        }

        var collider = projectile.GetComponent<Collider2D>();
        Assert.IsNotNull(collider);
        
        if (collider is CircleCollider2D circ)
        {
            _circleCollider2D = circ;
            _get_width_of_collider = () => _circleCollider2D.radius;
        }
        else if (collider is CapsuleCollider2D cap)
        {
            _capsuleCollider2D = cap;
            _get_width_of_collider = _capsuleCollider2D.direction switch
            {
                CapsuleDirection2D.Horizontal => () => _capsuleCollider2D.size.y,
                CapsuleDirection2D.Vertical => () => _capsuleCollider2D.size.x
            };
        }
        Assert.IsTrue(_circleCollider2D != null || _capsuleCollider2D != null);
    }

    

    private Action _Fire;
    /// <summary>
    /// Fire is called by the EnemyAI script, and FireProjectile() is called by the animation controller.
    /// </summary>
    public bool Fire(Transform target_pos, bool only_if_clear_line_of_sight = false, float? distance = null)
    {
        _target_pos = target_pos;
        if (only_if_clear_line_of_sight)
        {
            var firepoint = GetFirePoint.Invoke().position;

            var direction = (_target_pos.position - firepoint).normalized;
            Vector3.Distance(firepoint, _target_pos.position);
#if UNITY_EDITOR
            Debug.DrawLine(firepoint, target_pos.position, Color.blue, duration: 1f);
#endif
            var mask = LayerMask.GetMask("Props", "Walls", "Player");
            // mask = ~mask;
            
            
            var hit = Physics2D.Raycast(
                firepoint,
                direction,
                distance ?? Mathf.Infinity,
                mask
                );

            if (!(hit.collider != null && hit.rigidbody.gameObject.CompareTag("Player")))
            {
                Debug.Log("hit did NOT hit a collider");
                return false;
            }

            
            var width = _get_width_of_collider.Invoke();
            width += width * projectilePadding;
            direction.z = 0;
            var angle_of_projectile_direction = Util.GetAngleFromVectorFloat(direction);
            // Debug.Log($"Firepoint = {firepoint}");
            var direction_towards_perimeter = (Quaternion.Euler(new Vector3(0f, 0f, 90f)) * direction).normalized;
            
            var upper_start_ray_pos = firepoint + direction_towards_perimeter * width;
            var lower_start_ray_pos = firepoint - direction_towards_perimeter * width;
            var upper_end_ray_pos = _target_pos.position + direction_towards_perimeter * width;
            var lower_end_ray_pos = _target_pos.position - direction_towards_perimeter * width;

            
            var direction_of_upper_ray = (upper_end_ray_pos - upper_start_ray_pos).normalized;
            var direction_of_lower_ray = (lower_end_ray_pos - lower_start_ray_pos).normalized;

            // Debug.Log("Firing upper ray");
            var upper_hit = Physics2D.Raycast(
                upper_start_ray_pos,
                direction_of_upper_ray,
                distance ?? Mathf.Infinity,
                mask
            );
#if UNITY_EDITOR
            Debug.DrawLine(upper_start_ray_pos, upper_start_ray_pos + direction_of_upper_ray * 10, Color.red);
#endif
            //Debug.Log("Firing lower ray");
            var lower_hit = Physics2D.Raycast(
                lower_start_ray_pos,
                direction_of_lower_ray,
                distance ?? Mathf.Infinity,
                mask
            );
#if UNITY_EDITOR
            Debug.DrawLine(lower_start_ray_pos, lower_start_ray_pos + direction_of_lower_ray * 10, Color.yellow);
#endif
            if (upper_hit.collider != null)
            {
                var upper_ray_did_not_hit_Player = !upper_hit.rigidbody.gameObject.CompareTag("Player");
                //Debug.Log($"upper_hit.distance = {upper_hit.distance} distance to target: {Vector3.Distance(upper_start_ray_pos, upper_end_ray_pos)}, did upper ray not hit PLayer? = {upper_ray_did_not_hit_Player}");
                if (upper_hit.distance < Vector3.Distance(upper_start_ray_pos, upper_end_ray_pos) && upper_ray_did_not_hit_Player)
                {
                    //Debug.Log("upper ray did not hit the Player OR the distance was less than the distance to endpos");
                    return false;
                }
            }
            
            if (lower_hit.collider != null)
            {
                var lower_ray_did_not_hit_Player = !lower_hit.rigidbody.gameObject.CompareTag("Player");
                //Debug.Log($"lower_hit.distance = {lower_hit.distance} distance to target: {Vector3.Distance(lower_start_ray_pos, lower_end_ray_pos)} did lower ray not hit PLayer? = {lower_ray_did_not_hit_Player}");
                if (lower_hit.distance < Vector3.Distance(lower_start_ray_pos, lower_end_ray_pos) && lower_ray_did_not_hit_Player)
                {
                    // Debug.Log("lower ray did not hit the Player OR the distance was less than the distance to endpos");
                    return false;
                }
            }
        }
        
        _Fire.Invoke();
        return true;
    }

    private Transform _target_pos;
    
    public void FireProjectile()
    {
        var firepoint = GetFirePoint.Invoke();
        Assert.IsNotNull(_target_pos);
        Assert.IsNotNull(firepoint);
        var projectile_direction = (_target_pos.position -firepoint.position).normalized;
        var angle = Util.GetAngleFromVectorFloat(new Vector3(projectile_direction.x, projectile_direction.y, 0));

        Debug.DrawLine(firepoint.position, _target_pos.position, Color.magenta, duration: 1f);
        
        
        var instance = Instantiate(projectile, firepoint.position, Quaternion.Euler(0, 0, angle));
        instance.Setup(
            projectileVariant);
        
        OnEnemyRangedAttack?.Invoke(_enemyType);
        
        instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * speed, ForceMode2D.Impulse);
    }

}
