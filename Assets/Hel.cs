using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BossHealthController))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Hel : MonoBehaviour
{
    [Space] [Header("Features:")] [SerializeField]
    private bool spawnGravityZones;


    [Range(0f, 30f)]
    public float teleportationRadius;
    
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    private enum AngularDirection
    {
        Clockwise,
        CounterClockwise
    }
    
    public enum SpecialAttack
    {
        SpawnGravitationalCircles,
        IciclePincer,
        SwirlSpray,
        SpawnMinions,
        SpawnDarkSpikes,
    }

    [Min(5f)]
    public float specialAttackDelay;
    
    public event Action<SpecialAttack> OnHelDoSpecialAttack;


    [Min(0.3f)]
    public float basicAttackDelay;


    [Range(0, 180)]
    public int basicAttackConeWidth;
    
    public enum AttackPhases
    {
        Phase1, // 100 - 80
        Phase2, // 80 - 60
        Phase3, // 60 - 40
        Phase4, // 40 - 20
        Phase5, // 20 - 0
    }

    private AttackPhases _curr_phase = AttackPhases.Phase1;
    
    private IEnumerator _active_attack_phase_coroutine;

    private IEnumerator Phase1()
    {
        while (true)
        {
            SpawnGravityZones(5, 0.1f);
            yield return new WaitForSeconds(specialAttackDelay);
        }
    }
    
    private IEnumerator Phase2()
    {
        yield return null;
    }
    
    private IEnumerator Phase3()
    {
        while (true)
        {
            SpawnGravityZones(10, 0.1f);
            yield return new WaitForSeconds(specialAttackDelay);
        }
    }
    
    private IEnumerator Phase4()
    {
        yield return null;
    }
    
    private IEnumerator Phase5()
    {
        yield return null;
    }

    private bool _black_spikes_activated = false;

    private IEnumerator _teleportation_coroutine;

    private bool _teleport_phase_1_started = false;
    private bool _teleport_phase_2_started = false;

    private int _basic_attack_volley_amount = 1;
    private int _basic_attack_speed = 3;
    private void ChangeAttackPhaseBasedHealthpointRatio(BossHealthController.Boss boss, float curr_hp, float max_hp)
    {
        var ratio = curr_hp / max_hp;

        if (ratio <= 0.5f && !_black_spikes_activated)
        {
            Debug.Log("Casting spawn black spikes that follow player attack");
            // _animator.SetTrigger(SpawnBlackSpikes);
            SpawnDarkSpikesForever();
            _black_spikes_activated = true;
        }
        
        var (phase, basic_attack_volley_amount, basic_attack_speed, phase_num) = ratio switch
        {
            > 0.8f and <= 1.0f => (AttackPhases.Phase1, 1, 3, 1),
            > 0.6f and <= 0.8f => (AttackPhases.Phase2, 3, 6, 2),
            > 0.4f and <= 0.6f => (AttackPhases.Phase3, 5, 9, 3),
            > 0.2f and <= 0.4f => (AttackPhases.Phase4, 7, 12, 4),
            > 0 and <= 0.2f => (AttackPhases.Phase5, 9, 15, 5)
        };

        _basic_attack_volley_amount = basic_attack_volley_amount;
        _basic_attack_speed = basic_attack_speed;
        
        if (_curr_phase != phase)
        {
            Debug.Log($"starting phase! {phase_num}");
            _curr_phase = phase;
            /*
            if (_active_attack_phase_coroutine != null)
            {
                StopCoroutine(_active_attack_phase_coroutine);
            }
            _active_attack_phase_coroutine = _curr_phase switch
            {
                AttackPhases.Phase1 => Phase1(),
                AttackPhases.Phase2 => Phase2(),
                AttackPhases.Phase3 => Phase3(),
                AttackPhases.Phase4 => Phase4(),
                AttackPhases.Phase5 => Phase5()
            };
            StartCoroutine(_active_attack_phase_coroutine);
            */

            _available_special_attacks = _curr_phase switch
            {
                AttackPhases.Phase1 => new List<SpecialAttack> {SpecialAttack.IciclePincer},
                AttackPhases.Phase2 => new List<SpecialAttack> {SpecialAttack.IciclePincer, SpecialAttack.SpawnMinions},
                AttackPhases.Phase3 => new List<SpecialAttack> {SpecialAttack.SpawnGravitationalCircles},
                AttackPhases.Phase4 => new List<SpecialAttack> {SpecialAttack.IciclePincer, SpecialAttack.SpawnMinions, SpecialAttack.SwirlSpray},
                AttackPhases.Phase5 => new List<SpecialAttack> {SpecialAttack.IciclePincer, SpecialAttack.SpawnGravitationalCircles, SpecialAttack.SwirlSpray},
                _ => throw new ArgumentOutOfRangeException()
            };
            
            
            if (_curr_phase == AttackPhases.Phase2 && !_teleport_phase_1_started)
            {
                var repeat_rate = 4f;
                Debug.Log($"Start teleporting every {repeat_rate}");
                InvokeRepeating(nameof(TeleportForever), 0, repeat_rate);
                
                _teleport_phase_1_started = true;
            }
            else if (_curr_phase == AttackPhases.Phase4 && !_teleport_phase_2_started)
            {
                Debug.Log("Starting previous teleportation behaviour");
                CancelInvoke(nameof(TeleportForever));
                var repeat_rate = 2f;
                Debug.Log($"Start teleporting every {repeat_rate}");
                InvokeRepeating(nameof(TeleportForever), 1, repeat_rate);
                
                _teleport_phase_2_started = true;
            }
        }
    }

    private List<SpecialAttack> _available_special_attacks = new();

    private void DoSpecialAttack()
    {
        var special_attack = _available_special_attacks[Random.Range(0, _available_special_attacks.Count - 1)];

        switch (special_attack)
        {
            case SpecialAttack.IciclePincer:
                IciclePincer(25);
                
                break;
            case SpecialAttack.SpawnMinions:
                break;
            
            case SpecialAttack.SwirlSpray:
                
                break;
            
            case SpecialAttack.SpawnDarkSpikes:
                break;
            
            case SpecialAttack.SpawnGravitationalCircles:
                SpawnGravityZones(3, delay: 0.1f);
                break;
        }
    }
    
    private void OnEnable()
    {
        bhc.OnBossTakeDamage += ChangeAttackPhaseBasedHealthpointRatio;
    }

    private void OnDisable()
    {
        bhc.OnBossTakeDamage += ChangeAttackPhaseBasedHealthpointRatio;
    }

    public BossHealthController bhc;
    

    [SerializeField] private Projectile projectile;
    [Min(0f)]
    public float projectileSpeed;
    
    [Header("Should be Player/Toke")]
    public Transform target;
    
    [NotNull] private Animator _animator;

    public GravitySphere gravitySphere;
    [Range(0f, 5f)] public float destroyGravityZonesAfterDelay;

    public BlackSpike blackSpike;
    [Min(1)]
    public int blackSpikeDamage;
    
    private Transform _right_hand_firepoint;
    private Transform _left_hand_firepoint;

    public const string LEFT_HAND_FIREPOINT_NAME = "Left Hand Firepoint";
    public const string RIGHT_HAND_FIREPOINT_NAME = "Right Hand Firepoint";


    private Vector3 teleportationCenter;
    private static readonly int SpawnBlackSpikes = Animator.StringToHash("spawn black spikes");
    private static readonly int DoBasicAttack = Animator.StringToHash("basic attack");


    // -----------------------------------------------------------------------------------------------------------------
    void Start()
    {
        teleportationCenter = transform.position;
        blackSpike.damage = blackSpikeDamage;
        
        
        var child_transforms = GetComponentsInChildren<Transform>();
        _right_hand_firepoint = child_transforms.First(t => t.name == RIGHT_HAND_FIREPOINT_NAME);
        _left_hand_firepoint = child_transforms.First(t => t.name == LEFT_HAND_FIREPOINT_NAME);
        _animator = GetComponent<Animator>();
        // Assert.IsTrue(target.gameObject.CompareTag("Player"));

        InvokeRepeating(nameof(_BasicAttack), 3, basicAttackDelay);
        InvokeRepeating(nameof(DoSpecialAttack), 3, specialAttackDelay);
        // StartCoroutine(Test(5));
        // StartCoroutine(BasicAttackRoutine());
    }


    private void _BasicAttack()
    {
        Debug.Log("Hel do basic attack");
        _animator.SetTrigger(DoBasicAttack);
    }

    private void BasicAttack()
    {
        var direction = (target.position - transform.position).normalized;
        var half_cone_width = basicAttackConeWidth / 2f;
        var angle = Util.GetAngleFromVectorFloat(direction);
        var start_angle = angle - half_cone_width;
        var end_angle = angle + half_cone_width;
        
        Debug.Log($"Angle to target = {angle}");
        
        Debug.DrawLine(transform.position, transform.position + direction * 20);
        // FireConeOfProjectiles(angle, basicAttackConeWidth, (byte) _basic_attack_volley_amount, _basic_attack_speed);

        if (_basic_attack_volley_amount == 1)
        {
            start_angle = angle;
            end_angle = angle;
        }
        
        Debug.Log($"angle = {angle} start_angle = {start_angle} end_angle = {end_angle}");
        
        FireVolleyOfProjectiles(start_angle, end_angle, (byte) _basic_attack_volley_amount, _basic_attack_speed, use_late_night_hack: true);
    }
    
    /*private IEnumerator BasicAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(basicAttackDelay);
            _animator.SetTrigger(BasicAttack);
        }
    }*/
    

    private IEnumerator Test(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        
        // IciclePincer(50);
        // SpawnDarkSpikesProtrudingFromTheGround(150, radius: 2f, follow_target: true, delay: 0.1f);
        
        // CircularAttack(200, 5, Vector3.right, AngularDirection.Clockwise, 0.05f);

        //SpawnGravityZones(10, delay: 0.5f);
    }

   
    private IEnumerator TeleportForeverCoroutine(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            TeleportToRandomPositionWithinTeleportationCircle();
        }
    }
  
    
    
    private void TeleportForever() => TeleportToRandomPositionWithinTeleportationCircle();
    
    
    private void TeleportToRandomPositionWithinTeleportationCircle()
    {
        _animator.SetTrigger("teleport");
    }

    private void Teleport()
    {
        var teleportPos = Random.insideUnitCircle * teleportationRadius + (Vector2) teleportationCenter;
        transform.position = teleportPos;
    }
    

    private List<Vector2> SampleRandomPointWithinCircle(Vector2 pos, float radius, ushort n_samples) =>
        Enumerable.Range(0, n_samples)
            .Select(_ => pos + Random.insideUnitCircle * radius)
            .ToList();

    private IEnumerator _SpawnGravityZones(List<Vector2> spawning_points, float delay)
    {
        foreach (var spawn_point in spawning_points)
        {
            var instance = Instantiate(gravitySphere, spawn_point, Quaternion.identity);
            instance.Setup(lifetime: destroyGravityZonesAfterDelay);
            // Destroy(instance, destroyGravityZonesAfterDelay);
            yield return new WaitForSeconds(delay);
        }
    } 
    
    private void SpawnGravityZones(ushort n_zones, float delay = 0f)
    {
        var spawning_points = SampleRandomPointWithinCircle(teleportationCenter, teleportationRadius, n_zones);
        if (delay != 0f)
        {
            StartCoroutine(_SpawnGravityZones(spawning_points, delay));
        }
        else
        {
            foreach (var spawn_point in spawning_points)
            {
                var instance = Instantiate(gravitySphere, spawn_point, Quaternion.identity);
                instance.Setup(lifetime: destroyGravityZonesAfterDelay);
                // Destroy(instance, destroyGravityZonesAfterDelay);
            }
        }
    }
    
    private IEnumerator _SpawnBlackSpikes(ushort n_spikes, float radius, bool follow_target = true, float delay = 0f)
    {
        if (follow_target)
        {
            for (int i = 0; i < n_spikes; i++)
            {
                var spawn_point = Random.insideUnitCircle * radius + (Vector2) target.position;
                Instantiate(blackSpike, spawn_point, Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
        }
        else
        {
            var spawning_points = SampleRandomPointWithinCircle(target.position, radius, n_spikes);
            foreach (var spawn_point in spawning_points)
            {
                Instantiate(blackSpike, spawn_point, Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private void SpawnDarkSpikesForever() => StartCoroutine(_SpawnDarkSpikesForever(radius: 5f, delay: 0.01f));

    private IEnumerator _SpawnDarkSpikesForever(float radius, float delay)
    {
        while (true)
        {
            var spawn_point = Random.insideUnitCircle * radius + (Vector2) target.position;
            Instantiate(blackSpike, spawn_point, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
    }
    
    private void SpawnDarkSpikesProtrudingFromTheGround(ushort n_spikes, float radius, bool follow_target = true, float delay = 0f)
    {
        
        if (delay != 0f)
        {
            StartCoroutine(_SpawnBlackSpikes(n_spikes, radius, follow_target, delay));
        }
        else
        {
            var spawning_points = SampleRandomPointWithinCircle(target.position, radius, n_spikes);
            foreach (var spawn_point in spawning_points)
            {
                Instantiate(blackSpike, spawn_point, Quaternion.identity);
            }
        }
    }


    private Direction GetQuadrantInRelationToTarget(Vector3 target)
    {
        var direction = (target - transform.position).normalized;
        var angle = Util.GetAngleFromVectorFloat(direction);
        return angle switch
        {
            >= 0f and <= 45 or <= 0f and >= 315 => Direction.Left,
            > 45f and <= 135f => Direction.Up,
            > 135f and <= 225f => Direction.Right,
            > 225f and < 315f => Direction.Down,
            _ => throw new ArgumentException($"angle is invalid: {angle}")
        };
    }


    private IEnumerator _CircularAttack(int n_projectiles, float whole_rotations, Vector3 starting_direction,AngularDirection dir, float delay)
    {
        var angle = Util.GetAngleFromVectorFloat(starting_direction);
        var angular_increment = n_projectiles / whole_rotations;
        angular_increment = 360 / angular_increment;
        var direction = starting_direction;
        angular_increment *= dir switch
        {
            AngularDirection.Clockwise => -1,
            AngularDirection.CounterClockwise => 1
        };
            
            
        for (int i = 0; i < n_projectiles; i++)
        {
            var rot = Quaternion.Euler(0f, 0f, angle);
            direction = rot * Vector3.right;
            angle += angular_increment;
            
            var velocity = direction * projectileSpeed;
            var p = Instantiate(projectile, transform.position + direction, rot);
            p.Setup(Projectile.Variant.ENEMY);
            p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            yield return new WaitForSeconds(delay);
        }
        
    }


    private void CircularAttack(int n_projectiles, float whole_rotations, Vector3 starting_direction, AngularDirection dir, float delay = 0f)
    {
        var angle = Util.GetAngleFromVectorFloat(starting_direction);
        
        Assert.IsTrue(whole_rotations > 0);
        if (delay != 0f)
        {
            StartCoroutine(_CircularAttack(n_projectiles, whole_rotations, starting_direction, dir, delay));
        }
        else
        {
            var angular_increment = n_projectiles / whole_rotations;
            angular_increment = 360 / angular_increment;
            var direction = starting_direction;
            angular_increment *= dir switch
            {
                AngularDirection.Clockwise => -1,
                AngularDirection.CounterClockwise => 1
            };
            
            
            for (int i = 0; i < n_projectiles; i++)
            {
                var rot = Quaternion.Euler(0f, 0f, angle);
                direction = rot * Vector3.right;
                angle += angular_increment;
            
                var velocity = direction * projectileSpeed;
                var p = Instantiate(projectile, transform.position + direction, rot);
                p.Setup(Projectile.Variant.ENEMY);
                p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            }
        }
    }

    private void FireVolleyOfProjectiles(float start_angle, float end_angle, byte n_projectiles,
        float projectile_speed = 5f, bool use_late_night_hack = false)
    {
        Debug.Log($"in volley: start_angle = {start_angle} end_angle = {end_angle}");
        var direction = Vector3.right;
        var angular_increment = Mathf.Abs(start_angle - end_angle);
        angular_increment = angular_increment > 180 ? 360 - angular_increment : angular_increment;
        angular_increment /= n_projectiles == 1 ? 1 : n_projectiles - (use_late_night_hack ? 1 : 0);
        Debug.Log($"angular increment is {angular_increment}");
        var angle = start_angle;

        
        for (int i = 0; i < n_projectiles; i++)
        {
            var rot = Quaternion.Euler(0f, 0f, angle);
            direction = rot * Vector3.right;
            angle += angular_increment;
            
            var velocity = direction * projectile_speed;
            var p = Instantiate(projectile, transform.position + direction, rot);
            p.Setup(Projectile.Variant.ENEMY);
            p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
        }
    }

    private void FireConeOfProjectiles(float angle_to_target, float cone_width, byte n_projectiles,
        float projectile_speed = 5f)
    {
        var direction = Vector3.right;
        
        /*var angular_increment = Mathf.Abs(start_angle - end_angle);
        angular_increment = angular_increment > 180 ? 360 - angular_increment : angular_increment;
        angular_increment /= n_projectiles;
        */
        
        
        var angle = angle_to_target;

        {
            // fire directly at center of cone
            var rot = Quaternion.Euler(0f, 0f, angle);
            direction = rot * Vector3.right;
            var velocity = direction * projectile_speed;
            var p = Instantiate(projectile, transform.position + direction, rot);
            p.Setup(Projectile.Variant.ENEMY);
            p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
        }

        var n_projectiles_in_half_cone = (n_projectiles - 1) / 2;
        var half_cone_width = cone_width / 2;
        var left_start_angle = (half_cone_width + angle) % 360;
        var right_start_angle = (half_cone_width + angle) % 360;
        
        var angle_increment = half_cone_width / n_projectiles_in_half_cone;
        {
            var start_angle = left_start_angle;
            for (int i = 0; i < n_projectiles_in_half_cone; i++)
            {
                var rot = Quaternion.Euler(0f, 0f, start_angle);
                direction = rot * Vector3.right;
                start_angle += angle_increment;
                var velocity = direction * projectile_speed;
                var p = Instantiate(projectile, transform.position + direction, rot);
                p.Setup(Projectile.Variant.ENEMY);
                p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            }
        }
        
        {
            var start_angle = right_start_angle;
            for (int i = 0; i < n_projectiles_in_half_cone; i++)
            {
                var rot = Quaternion.Euler(0f, 0f, start_angle);
                direction = rot * Vector3.right;
                start_angle -= angle_increment;
                var velocity = direction * projectile_speed;
                var p = Instantiate(projectile, transform.position + direction, rot);
                p.Setup(Projectile.Variant.ENEMY);
                p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            }
        }
    }
    
    private IEnumerator FireVolleyOfProjectilesCoroutine(AngularDirection dir, float start_angle, float end_angle, byte n_projectiles, float delay, float projectile_speed = 5f)
    {
        Debug.Log($"clockwise? {dir == AngularDirection.Clockwise} start_angle = {start_angle} end_angle = {end_angle}");
        
        
        var direction = Vector3.right;
        var angular_increment = Mathf.Abs(start_angle - end_angle);
        angular_increment = angular_increment > 180 ? 360 - angular_increment : angular_increment;
        angular_increment /= n_projectiles;
        Debug.Log($"angular increment = {angular_increment}");
        var angle = start_angle;
        angular_increment *= dir switch
        {
            AngularDirection.Clockwise => -1,
            AngularDirection.CounterClockwise => 1
        };
        
        // var rot = Quaternion.Euler(0f, 0f, angular_increment);
        
        for (int i = 0; i < n_projectiles; i++)
        {
            var rot = Quaternion.Euler(0f, 0f, angle);
            direction = rot * Vector3.right;
            angle += angular_increment;
            
            var velocity = direction * projectile_speed;
            var p = Instantiate(projectile, transform.position + direction, rot);
            p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            p.Setup(Projectile.Variant.ENEMY);
            yield return new WaitForSeconds(delay);
        }
    }
    
    private void IciclePincer(byte n_projectiles)
    {
        var direction = (target.position - transform.position).normalized;
        var angle = Util.GetAngleFromVectorFloat(direction);
        var left = ((int) angle + 90) % 360;
        var right =((int) angle - 90) % 360;
        Debug.Log($"angle to target: {angle}, left: {left}, right: {right}");

        StartCoroutine(FireVolleyOfProjectilesCoroutine(AngularDirection.Clockwise, left, angle - 45, n_projectiles, 0f));
        StartCoroutine(FireVolleyOfProjectilesCoroutine(AngularDirection.CounterClockwise, right, angle + 45, n_projectiles, 0f));
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, teleportationRadius);
    }
}
