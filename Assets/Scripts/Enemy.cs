using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public UnityEvent took_damage;
    
    [SerializeField]
    private int hp = 100;
    
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            var trigger = "death";
            anim.SetTrigger(trigger);
        }
    }

    public void TakeDamage(int dmg)
    {
        var trigger = "get hit";
        hp -= dmg;
        if (hp <= 0)
        {
            trigger = "death";
        }
        Debug.Log($"off i took damage my health is {hp}");
        
        anim.SetTrigger(trigger);
        
        
        Debug.Log("calling event took_damage");
        took_damage?.Invoke();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
