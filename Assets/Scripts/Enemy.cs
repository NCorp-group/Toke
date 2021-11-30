using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemySpawn;
    public static event Action OnEnemyDie;
    
    public int hp = 100;
    
    private Animator anim;

    [SerializeField] private bool dropCollectableOnDeath = false;
    [Range(0.0f, 1.0f)] public float likelihood = 1.0f;
    public Collectable collectable;

    
    private void Start()
    {
        anim = GetComponent<Animator>();
        
        OnEnemySpawn?.Invoke();
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
    }

    private void Die()
    {
        OnEnemyDie?.Invoke();
        if (dropCollectableOnDeath)
        {
            var rand = Random.Range(0.0f, 1.0f);
            if (rand <= likelihood)
            {
                Instantiate(collectable, transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }
    
}
