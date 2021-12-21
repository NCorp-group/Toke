using System;
using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

// TODO: super cool
//[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class MeleeAttack : MonoBehaviour
{
    public static event Action<Enemy.EnemyType> OnEnemyMeleeAttack;
    private Enemy.EnemyType _enemyType;

    public int damage = 10;
    
    [Header("If true, then a child object with a Collider2D will be used.\nThe name MUST be \"Melee Attack Area\".\nOtherwise use the transform of this object")]
    [SerializeField] private bool useChildColliderForAttackArea = false;
    [Header("Only used if \"Use Child Collider For Attack Area\" is set to true")]
    [SerializeField] private float attackRadius;
    public LayerMask attackLayers;

    [Header("A delay in (real world) seconds between consecutive attacks.")]
    [Range(0f, 10f)]
    [SerializeField] private float attackDelay = 0f;
    
    [Header("If true then responsibility of calling \"Attack()\"\nis delegated to an animation event on the object,\nwith trigger named \"attack\"")]
    [SerializeField] private bool useAnimationEventToTriggerAttack = false;


    private Action _apply_delay;
    private bool can_attack = true;

    private Animator _animator;
    private Action _Attack;
    
    public static readonly string GAMEOBJECT_NAME_FOR_ATTACK_AREA = "Melee Attack Area";

    // private Func<Collider2D[]> _get_attack_colliders;
    private Func<bool> _get_attack_colliders;

    private Collider2D _attack_area_collider = null;
    // FIX: set default size to like 4
    private Collider2D[] _results = new Collider2D[4];
    
    
    // TODO: cache reference
    [CanBeNull] private Stats _stats = null;
    //[CanBeNull] private PlayerHealthController _phc = null;
    private static readonly int AttackTrigger = Animator.StringToHash("attack");

    private IEnumerator WaitDelayForConsecutiveAttack(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        can_attack = true;
    }
    
    void Start()
    {
        _enemyType = GetComponent<Enemy>().type;

        Assert.IsTrue(damage >= 0, "damage >= 0");
        Assert.IsTrue(attackRadius >= 0, "attackRadius >= 0");

        //_phc = GameObject.FindWithTag("Player").GetComponent<PlayerHealthController>();
        _stats = GameObject.FindWithTag("Player").GetComponent<Stats>();
        
        _apply_delay = attackDelay switch
        {
            0 => () => { },
            _ => () =>
            {
                Debug.Log($"applying {attackDelay} delay");
                StartCoroutine(WaitDelayForConsecutiveAttack(attackDelay));
            }
        };

        _Attack = useAnimationEventToTriggerAttack switch
        {
            false => () => Attack(),
            true => () => _animator.SetTrigger(AttackTrigger)
        };
        
        if (useChildColliderForAttackArea)
        {
            _attack_area_collider = GetComponentsInChildren<Collider2D>()
                .FirstOrDefault(coll => coll.gameObject.name == GAMEOBJECT_NAME_FOR_ATTACK_AREA);
            Assert.IsNotNull(_attack_area_collider, "_attack_area != null");
        }

        _animator = GetComponent<Animator>();
        
        _get_attack_colliders = useChildColliderForAttackArea 
            
            ? () =>
            {
                var contact_filter = new ContactFilter2D();
                contact_filter.layerMask = attackLayers;
                contact_filter.useLayerMask = true;
                var n_collisions = Physics2D.OverlapCollider(_attack_area_collider, contact_filter, _results);
                var player_hit = n_collisions != 0;
                return player_hit;
                /*
                if (player_hit)
                {
                    
                    var res = _results.Select(x => x).ToArray();
                    _results.
                }

                return _results;
                */
            }
            : () => Physics2D.OverlapCircleAll(transform.position, attackRadius, attackLayers).Length != 0;
    }
    
    
    public void Attack()
    {
        _apply_delay.Invoke();
        var hit_player =_get_attack_colliders.Invoke();
        OnEnemyMeleeAttack?.Invoke(_enemyType);
        if (!hit_player)
        {
            Debug.Log("No colliders hit in melee attack");
            return;
        }
        
        _stats.TakeDamage(damage);
    }

    private void OnDrawGizmosSelected()
    {
        if (useChildColliderForAttackArea) return;

        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
    
    public bool CheckAttack()
    {
        if (!can_attack)
        {
            Debug.Log("can't attack yet due to delay");
            return false;
        }
        var hit_player =_get_attack_colliders.Invoke();
        if (!hit_player)
        {
            Debug.Log("No colliders hit in melee attack");
            return false;
        }

        can_attack = false;
        /*
        var colliders = _get_attack_colliders.Invoke();
        if (colliders.Length == 0)
        {
            Debug.Log("No colliders hit in melee attack");
            return false;
        }
        */

        /*
        var player_collider = colliders
            .FirstOrDefault(coll => coll.gameObject.CompareTag("Player"));
        _phc ??= player_collider.gameObject.GetComponent<PlayerHealthController>();
        */
        Debug.Log("calling _Attack");
        _Attack.Invoke();
        return true;
    }
}
