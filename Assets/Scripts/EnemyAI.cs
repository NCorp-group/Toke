using System;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Assertions;
using Quaternion = UnityEngine.Quaternion;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float aggressionRadius = 10f;
    [Header("If not specified, then \"Player\" is selected per default")]
    [SerializeField] private Transform target;

    [Header("The point from which the distance is measured to the target. If none, then the objects transform is used")]
    [SerializeField] private Transform from;

    private AIPath _ai_path;
    private AIDestinationSetter _aiDestinationSetter;
    
    public enum EnemyType
    {
        /// <summary>
        /// If within aggression range, then move towards target until with attack radius, then attack.
        /// </summary>
        Melee,
        
        /// <summary>
        /// If within aggression range, then check if there is a clear line 
        /// of site for a projectile. If there is stay and fire, else
        /// move towards target.
        /// </summary>
        Ranged,
    };

    public EnemyType enemyType = EnemyType.Ranged;
    
    private bool RangedBehaviour()
    {
        var a = 2;
        var clear_line_of_sight = _rangedAttack.Fire(
            target,
            _target_rb2d.velocity,
            only_if_clear_line_of_sight: true,
            distance: aggressionRadius
            );

        return clear_line_of_sight;
    }
    
    private bool MeleeBehaviour()
    {
        return _meleeAttack.CheckAttack();
    }

    private Func<bool> _behaviour;

    private RangedAttack _rangedAttack;
    private MeleeAttack _meleeAttack;

    private Animator _animator;
    private Rigidbody2D _rb2d;
    private Action _inform_animator_about_speed;
    private static readonly int Speed = Animator.StringToHash("speed");

    private Rigidbody2D _target_rb2d;
    
    void Start()
    {
        _aiDestinationSetter = GetComponent<AIDestinationSetter>();

        _animator = GetComponent<Animator>();
        _rb2d = GetComponentInChildren<Rigidbody2D>();
        
        _inform_animator_about_speed = _rb2d switch
        {
            null => () => { },
            _ => () =>
            {
                var speed = _ai_path.velocity.magnitude;
                Debug.Log($"speed of rb2d is = {speed}");
                _animator.SetFloat(Speed, speed);
            }
        };
        
        Assert.IsTrue(aggressionRadius > 1f);
        
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Debug.Log("finding child object called Target Point");
                target = player.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Target Point");
                _target_rb2d = player.GetComponent<Rigidbody2D>();
                Assert.IsNotNull(target);
            }
        }
        Debug.Log($"The name of the target is {target.gameObject.name}");
        _aiDestinationSetter.target = target.transform;


        switch (enemyType)
        {
            case EnemyType.Ranged:
                _rangedAttack = GetComponent<RangedAttack>();
                Assert.IsNotNull(_rangedAttack);
                Debug.Log("ranged attack is NOT null");
                _behaviour = RangedBehaviour;
                break;
            case EnemyType.Melee:
                _meleeAttack = GetComponent<MeleeAttack>();
                Assert.IsNotNull(_meleeAttack);
                _behaviour = MeleeBehaviour;
                break;
        }
        
        from = transform;
        _ai_path = GetComponent<AIPath>();
        Assert.IsNotNull(_ai_path, "_ai_path != null");
        // from = from ?? transform;
    }

    private void SetAiState(bool state) => _ai_path.canMove = state;

    private void EnableAI()
    {
        if (_can_move) SetAiState(true);
    } 
    private void DisableAI() =>  SetAiState(false);

    private bool _can_move = true;
    public void StopMovement()
    {
        Debug.Log("call StopMovement()");
        _can_move = false;
        DisableAI();
    }

    public void StartMovement()
    {
        Debug.Log("call StartMovement()");
        _can_move = true;
        EnableAI();
    }
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"DESIRED VELOCITY = {aiPath.desiredVelocity}");
        var ai_enabled = _ai_path.canMove == true;
        /*if (ai_enabled)
        {
            Debug.Log($"steering target.y = {aiPath.steeringTarget.y}");
            Debug.DrawLine(transform.position, aiPath.steeringTarget, Color.cyan);
        }*/
        transform.localScale = new Vector2(
            ai_enabled
                ? ((_ai_path.steeringTarget - from.position).x >= 0.01 ? 1 : -1)
                : ((target.position - from.position).x >= 0.01 ? 1 : -1),
            1
        );
        
        _inform_animator_about_speed.Invoke();
        
        Debug.Log($"can move = {_can_move}");

        var distance_to_target = UnityEngine.Vector2.Distance(from.position, target.position);
        var target_within_aggression_range = distance_to_target <= aggressionRadius;
        // Debug.Log($"distance to target = {distance_to_target}");
        //if (target_within_aggression_range && _behaviour.Invoke() && !_ai_path.canMove)
        if (target_within_aggression_range && _behaviour.Invoke())
        {
            //Debug.Log("target within distance, enabling attack behaviour");
            //_behaviour.Invoke();
            DisableAI();
        }
        else
        {
            //Debug.Log("target not within distance, enabling AI");
            EnableAI();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRadius);
    }
}
