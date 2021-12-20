using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Hel : MonoBehaviour
{
    public Transform[] teleportPoints;

    [Space] [Header("Features:")] [SerializeField]
    private bool spawnGravityZones;


    [SerializeField] private Projectile projectile;

    [Header("Should be Player/Toke")]
    public Transform target;
    
    [NotNull] private Animator _animator;
    private Collider2D _collider2d;

    public GravitySink gravitySink;
    [Range(0f, 5f)] public float destroyGravityZonesAfterDelay;
    
    
    [NotNull] private Transform _right_hand_firepoint;
    [NotNull] private Transform _left_hand_firepoint;

    public const string LEFT_HAND_FIREPOINT_NAME = "Left Hand Firepoint name";
    public const string RIGHT_HAND_FIREPOINT_NAME = "Right Hand Firepoint name";


    private bool _vulnerable = true;

    // ------------------------------------------------------------------------------------------
    void Start()
    {
        var child_transforms = GetComponentsInChildren<Transform>();
        _right_hand_firepoint = child_transforms.First(t => t.name == RIGHT_HAND_FIREPOINT_NAME);
        _left_hand_firepoint = child_transforms.First(t => t.name == LEFT_HAND_FIREPOINT_NAME);
        _animator = GetComponent<Animator>();
        Assert.IsTrue(target.gameObject.CompareTag("Player"));

        StartCoroutine(Test(5));
    }

    private IEnumerator Test(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnGravityZones(5);
    }

    private void SetVulnerability(bool vulnerable = true)
    {
        _vulnerable = vulnerable;
        _collider2d.enabled = vulnerable;
    }

    private void MakeVulnerable() => SetVulnerability(vulnerable: true);
    private void MakeInVulnerable() => SetVulnerability(vulnerable: false);


    private List<Vector2> SampleRandomPointWithinCircle(Vector2 pos, float radius, ushort n_samples) =>
        Enumerable.Range(0, n_samples)
            .Select(_ => pos + Random.insideUnitCircle * radius)
            .ToList();

    private IEnumerator _SpawnGravityZones(List<Vector2> spawning_points, float delay)
    {
        foreach (var spawn_point in spawning_points)
        {
            var instance = Instantiate(gravitySink, spawn_point, Quaternion.identity);
            Destroy(instance, destroyGravityZonesAfterDelay);
            yield return new WaitForSeconds(delay);
        }
    } 
    
    private void SpawnGravityZones(ushort n_zones, float delay = 0f)
    {
        var spawning_points = SampleRandomPointWithinCircle(transform.position, 10, n_zones);
        if (delay == 0f)
        {
            StartCoroutine(_SpawnGravityZones(spawning_points, delay));
        }
        else
        {
            foreach (var spawn_point in spawning_points)
            {
                var instance = Instantiate(gravitySink, spawn_point, Quaternion.identity);
                Destroy(instance, destroyGravityZonesAfterDelay);
            }
        }
    }
    
    private void SpawnDarkSpikesProtrudingFromTheGround(ushort n_spikes)
    {
        // 1: sample n number of points within a circle
        // foreach point, instantiate the object.
        
        var pos = Random.insideUnitCircle;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
