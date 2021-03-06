using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class GravitySphere : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Collapse = Animator.StringToHash("collapse");

    public static event Action OnGravitySphereSpawn;

    void Start()
    {
        _animator = GetComponent<Animator>();
        OnGravitySphereSpawn?.Invoke();
    }

    public void Setup(float lifetime)
    {
        StartCoroutine(CollapseAfterDelay(lifetime));
    }

    private IEnumerator CollapseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Setting collapse trigger og gravity sphere");
        _animator.SetTrigger(Collapse);
    }
    
    public void DestroySelf() => Destroy(gameObject);
}
