using System;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    private Action collect;
    public static event Func<int> OnItemCanBeCollected;
    public static event Action<Collectable> OnItemCollected;
    public static event Action<GameObject> OnProjectileAcquired; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_item == null ? "I don't have an item" : "I have picked an item");
        
        collect?.Invoke();
    }

    // expose collect function
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("entering collectable zone");
        var collectable = other.GetComponent<Collectable>();
        if (other.gameObject.CompareTag("Collectable") && collectable != null)
        {
            collect = Collect(collectable);
        }
    }

    // deexpose collect function
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("leaving collectable zone");
        collect = () => { };
        
        // collect = null;
    }

    private Action Collect(Collectable collectable)
    {
        return () =>
        {
            if (Input.GetKey(KeyCode.E))
            {
                var item = collectable.Collect();
                if (collectable.variant == Collectable.Variant.PROJECTILE)
                {
                    OnProjectileAcquired?.Invoke(item);
                    OnItemCollected?.Invoke(collectable);
                }
            }
        };
    }
}
