using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


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
    
    public event Action<SpecialAttack> OnHelDoSpecialAttack;

    
    public enum AttackPhases
    {
        Phase1, // 100 - 80
        Phase2, // 80 - 60
        Phase3, // 60 - 40
        Phase4, // 40 - 20
        Phase5, // 20 - 0
    }
    
    private IEnumerator _active_attack_phase_coroutine;

    private IEnumerator Phase1()
    {
        yield return null;
    }
    
    private IEnumerator Phase2()
    {
        yield return null;
    }
    
    private IEnumerator Phase3()
    {
        yield return null;
    }
    
    private IEnumerator Phase4()
    {
        yield return null;
    }
    
    private IEnumerator Phase5()
    {
        yield return null;
    }
    
    
    
    [SerializeField] private Projectile projectile;
    [Min(0f)]
    public float projectileSpeed;
    
    [Header("Should be Player/Toke")]
    public Transform target;
    
    [NotNull] private Animator _animator;

    public GravitySphere gravitySphere;
    [Range(0f, 5f)] public float destroyGravityZonesAfterDelay;

    public BlackSpike blackSpike;
    
    
    private Transform _right_hand_firepoint;
    private Transform _left_hand_firepoint;

    public const string LEFT_HAND_FIREPOINT_NAME = "Left Hand Firepoint";
    public const string RIGHT_HAND_FIREPOINT_NAME = "Right Hand Firepoint";


    private Vector3 teleportationCenter;
    
    
 
    
    
    // -----------------------------------------------------------------------------------------------------------------
    void Start()
    {
        teleportationCenter = transform.position;
        
        var child_transforms = GetComponentsInChildren<Transform>();
        _right_hand_firepoint = child_transforms.First(t => t.name == RIGHT_HAND_FIREPOINT_NAME);
        _left_hand_firepoint = child_transforms.First(t => t.name == LEFT_HAND_FIREPOINT_NAME);
        _animator = GetComponent<Animator>();
        // Assert.IsTrue(target.gameObject.CompareTag("Player"));

        StartCoroutine(Test(5));
    }

    private IEnumerator Test(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        
        // IciclePincer(50);
        SpawnDarkSpikesProtrudingFromTheGround(150, radius: 2f, follow_target: true, delay: 0.1f);
        StartCoroutine(TeleportForever(4));
        // CircularAttack(200, 5, Vector3.right, AngularDirection.Clockwise, 0.05f);

        //SpawnGravityZones(10, delay: 0.5f);
    }

   
    
  
    private IEnumerator TeleportForever(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            TeleportToRandomPositionWithinTeleportationCircle();
        }
    }
    
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
        var spawning_points = SampleRandomPointWithinCircle(transform.position, 15, n_zones);
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
                p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator FireVolleyOfProjectiles(AngularDirection dir, float start_angle, float end_angle, byte n_projectiles, float delay)
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
            
            var velocity = direction * projectileSpeed;
            var p = Instantiate(projectile, transform.position + direction, rot);
            p.GetComponent<Rigidbody2D>().AddForce(velocity, ForceMode2D.Impulse);
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

        StartCoroutine(FireVolleyOfProjectiles(AngularDirection.Clockwise, left, angle - 45, n_projectiles, 0f));
        StartCoroutine(FireVolleyOfProjectiles(AngularDirection.CounterClockwise, right, angle + 45, n_projectiles, 0f));
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, teleportationRadius);
    }
}
