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
        var clear_line_of_sight = _rangedAttack.Fire(
            target,
            only_if_clear_line_of_sight: true,
            distance: aggressionRadius
            );

        return clear_line_of_sight;
    }
    
    private bool MeleeBehaviour()
    {
        return true;
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
                _behaviour = RangedBehaviour;
                _rangedAttack = GetComponent<RangedAttack>();
                Assert.IsNotNull(_rangedAttack);
                break;
            case EnemyType.Melee:
                _behaviour = MeleeBehaviour;
                _meleeAttack = GetComponent<MeleeAttack>();
                Assert.IsNotNull(_meleeAttack);
                break;
        }
        Assert.IsNotNull(_behaviour);
    }

    private void SetAiState(bool state) => GetComponent<AIPath>().canMove = state;
    private void EnableAI() => SetAiState(true);
    private void DisableAI() => SetAiState(false);

    public AIPath aiPath;
    
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"DESIRED VELOCITY = {aiPath.desiredVelocity}");
        var ai_enabled = aiPath.canMove == true;
        /*if (ai_enabled)
        {
            Debug.Log($"steering target.y = {aiPath.steeringTarget.y}");
            Debug.DrawLine(transform.position, aiPath.steeringTarget, Color.cyan);
        }*/
        transform.localScale = new Vector3(
            ai_enabled
                ? ((aiPath.steeringTarget - transform.position).x >= 0.01 ? 1 : -1)
                : ((target.position - transform.position).x >= 0.01 ? 1 : -1),
            1,
            1
        );
        
        /*if (aiPath.desiredVelocity.x >= 0.01f)
        {
            Debug.Log("FACE RIGHT");
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            Debug.Log("FACE LEFT");
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }*/
        
        var distance_to_target = UnityEngine.Vector2.Distance(transform.position, target.position);
        var target_within_aggression_range = distance_to_target <= aggressionRadius;
        // Debug.Log($"distance to target = {distance_to_target}");
        if (target_within_aggression_range && _behaviour.Invoke())
        {
            Debug.Log("target within distance, enabling attack behaviour");
            //_behaviour.Invoke();
            DisableAI();
        }
        else
        {
            Debug.Log("target not within distance, enabling AI");
            EnableAI();
        }
    }
}
