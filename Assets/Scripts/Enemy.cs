using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;


[RequireComponent(typeof(FlashColorOnTakeDamage))]
public class Enemy : MonoBehaviour
{
    public static event Action<EnemyType> OnEnemySpawn;
    public static event Action<EnemyType> OnEnemyDie;
    public static event Action<EnemyType> OnEnemyTakeDamage;
    public event Action OnIndividualEnemyTakeDamage;
    
    public enum EnemyType 
    {
        SLIME, 
        WORM,
        BLUESLIME,
        NIGHTBORNIMP,
        ARCANEARCHER,
        EVILWIZARD
    }

    public EnemyType type = EnemyType.SLIME;
    public float hp = 100;
    
    private Animator anim;
    public static readonly string GAMEOBJECT_NAME_FOR_HIT_BODY = "Hit Body";


    [SerializeField] private bool dropCollectableOnDeath = true;
    private Collider2D _collider;
    [Range(0.0f, 1.0f)] public float likelihood = 1.0f;
    public Collectable collectable;

    private bool dead = false;
    private static readonly int GetHit = Animator.StringToHash("get hit");
    private static readonly int Death = Animator.StringToHash("death");

    private void Start()
    {

        // _collider = GetComponent<Collider2D>();
        _collider = GetComponentsInChildren<Collider2D>()
            .FirstOrDefault(coll => coll.gameObject.name == GAMEOBJECT_NAME_FOR_HIT_BODY);
        Assert.IsNotNull(_collider, "_collider != null");
        anim = GetComponent<Animator>();
        
        OnEnemySpawn?.Invoke(type);
    }

    
    public void TakeDamage(float dmg)
    {
        OnEnemyTakeDamage?.Invoke(type);
        OnIndividualEnemyTakeDamage?.Invoke();
        var trigger = GetHit;
        hp -= dmg;
        if (hp <= 0)
        {
            dead = true;
            if (dead)
            {
                OnEnemyDie?.Invoke(type);
                _collider.gameObject.SetActive(false);
                //Debug.Log("setting trigger DEATH");
                trigger = Death;
            }
        }
        anim.SetTrigger(trigger);
    }

    private void Die()
    {
        //Debug.Log("DIE");
        //collider.SetActive(false);
        /*if (dropCollectableOnDeath)
        {
            GetComponent<Loottable>().DropLoot();
            /*var rand = Random.Range(0.0f, 1.0f);
            if (rand <= likelihood)
            {
                Instantiate(collectable, transform.position, Quaternion.identity);
            }
        }*/
        Destroy(gameObject);
    }
}
