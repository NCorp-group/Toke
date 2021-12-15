using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class RangedAttack : MonoBehaviour
{
    public Projectile projectile;

    public float speed = 5f;
    public float lifetime = 10f;

    [Header("This requires a child object with tag = \"FirePoint\"")]
    [SerializeField] private bool useFirePoint = true;

    [Header("If true then responsibility of calling \"Fire()\" is delegated to an animation event on the object, with trigger named \"attack\"")]
    [SerializeField] private bool useAnimationEventToTriggerProjectileFire = false;

    [SerializeField] private Projectile.Variant projectileVariant = Projectile.Variant.ENEMY;
    
    
    private Transform _firepoint;
    private Animator _animator;
    private SpriteRenderer _sr;

    private Transform _child_firepoint;
    private Func<Transform> GetFirePoint;
    
    // Start is called before the first frame update
    private void Start()
    {
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
            var position = transform.position;
            var direction = (_target_pos.position - position).normalized;
            Debug.DrawLine(position, direction * 10, Color.red);
            var mask = LayerMask.GetMask("Props", "Walls", "Player");
            // mask = ~mask;
            
            var hit = Physics2D.Raycast(
                position,
                direction,
                distance ?? Mathf.Infinity,
                mask
                );
            if (!(hit.collider != null && hit.rigidbody.gameObject.CompareTag("Player")))
            {
                return false;
            }
        }
        
        _Fire.Invoke();
        return true;
    }

    private Transform _target_pos;
    
    public void FireProjectile()
    {
        var firepoint = GetFirePoint.Invoke();
        Debug.Log($"FIREPOINT = {firepoint.position}");
        Debug.Log($"TARGET = {_target_pos.position}");
        
        Assert.IsNotNull(_target_pos);
        Assert.IsNotNull(firepoint);
        var projectile_direction = (_target_pos.position -firepoint.position).normalized;
        var angle = Util.GetAngleFromVectorFloat(new Vector3(projectile_direction.x, projectile_direction.y, 0));
        
        var instance = Instantiate(projectile, firepoint.position, Quaternion.Euler(0, 0, angle));
        instance.Setup(
            projectileVariant);

        instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * speed, ForceMode2D.Impulse);
    }

}
