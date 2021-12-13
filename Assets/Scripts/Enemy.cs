using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : MonoBehaviour
{
    public static event Action<EnemyType> OnEnemySpawn;
    public static event Action<EnemyType> OnEnemyDie;
    public static event Action<EnemyType> OnEnemyTakeDamage;

    public event Action OnIndividualEnemyTakeDamage;
    
    public enum EnemyType 
    {
        SLIME, 
        WORM
    }

    public EnemyType type = EnemyType.SLIME;
    public int hp = 100;
    
    private Animator anim;

    [SerializeField] private bool dropCollectableOnDeath = false;
    [SerializeField] private GameObject collider;
    [Range(0.0f, 1.0f)] public float likelihood = 1.0f;
    public Collectable collectable;

    
    private void Start()
    {
        //int damage = 10;
        //OnEnemyTakeDamage?.Invoke(type);
        
        
        
        
        anim = GetComponent<Animator>();
        
        OnEnemySpawn?.Invoke(type);
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
        OnEnemyTakeDamage?.Invoke(type);
        OnIndividualEnemyTakeDamage?.Invoke();
        var trigger = "get hit";
        hp -= dmg;
        if (hp <= 0)
        {
            OnEnemyDie?.Invoke(type);
            collider.SetActive(false);
            trigger = "death";
        }
        //Debug.Log($"off i took damage my health is {hp}");
        
        anim.SetTrigger(trigger);
    }

    private void Die()
    {
        //collider.SetActive(false);
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
