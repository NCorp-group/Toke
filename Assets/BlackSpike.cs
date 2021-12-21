using UnityEngine;
using System;

public class BlackSpike : MonoBehaviour
{
    [Range(0f, 2f)]
    public float radius;

    public int damage;

    public LayerMask layerMask;

    private ContactFilter2D _contactFilter2D;

    public static event Action OnSpikeSpawn;

    // Start is called before the first frame update
    void Start()
    {
        _contactFilter2D = new ContactFilter2D();
        _contactFilter2D.layerMask = layerMask;
        _contactFilter2D.useLayerMask = true;
    }

    public void HasSpawned()
    {
        OnSpikeSpawn?.Invoke();
    }
    
    public void DealDamage()
    {
        var collider = Physics2D.OverlapCircle(transform.position, radius, layerMask);
        if (collider is not null)
        {
            collider.GetComponent<Stats>().TakeDamage(damage);
        }
    }

    public void DestroySelf() => Destroy(gameObject);

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
