using System;
using System.Linq;
using Pathfinding;
using UnityEngine;
using UnityEngine.Assertions;
using Quaternion = UnityEngine.Quaternion;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float aggressionRadius = 10f;
    [Header("If not specified, then \"Player\" is selected per default")]
    [SerializeField] private Transform target;

    [Header("The point from which the distance is measured to the target. If none, then the objects transform is used")]
    [SerializeField] private Transform from;

    private AIPath _ai_path;
    
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
        Assert.IsNotNull(target, "target != null");
        Assert.IsNotNull(_rangedAttack, "_rangedAttack != null");
        var clear_line_of_sight = _rangedAttack.Fire(
            target,
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
    
    void Start()
    {
        Assert.IsTrue(aggressionRadius > 1f);
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == "Target Point");
                Assert.IsNotNull(target);
            }
            
        }

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
        Assert.IsNotNull(_behaviour);

        from = transform;
        _ai_path = GetComponent<AIPath>();
        Assert.IsNotNull(_ai_path, "_ai_path != null");
        // from = from ?? transform;
    }

    private void SetAiState(bool state) => _ai_path.canMove = state;
    private void EnableAI() => SetAiState(true);
    private void DisableAI() => SetAiState(false);
    
    
    
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
        transform.localScale = new Vector3(
            ai_enabled
                ? ((_ai_path.steeringTarget - from.position).x >= 0.01 ? 1 : -1)
                : ((target.position - from.position).x >= 0.01 ? 1 : -1),
            1,
            1
        );
        
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
