using UnityEngine;

public class BlackSpike : MonoBehaviour
{
    [Range(0f, 2f)]
    public float radius;

    public int damage;

    public LayerMask layerMask;

    private ContactFilter2D _contactFilter2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _contactFilter2D = new ContactFilter2D();
        _contactFilter2D.layerMask = layerMask;
        _contactFilter2D.useLayerMask = true;
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
