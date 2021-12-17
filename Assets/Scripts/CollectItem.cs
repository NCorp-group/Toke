using System;
using UnityEngine;
using dpc = DoorPreviewController;

public class CollectItem : MonoBehaviour
{
    private Action interact;
    public static event Func<int> OnItemCanBeCollected;
    public static event Action<Collectable> OnItemCollected;
    public static event Action<GameObject> OnProjectileAcquired;
    public static event Action<dpc.RoomType> OnDoorInteraction;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_item == null ? "I don't have an item" : "I have picked an item");
        
        interact?.Invoke();
    }

    // expose collect function
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("entering collectable zone");
        if (other.gameObject.CompareTag("Collectable"))
        {
            Debug.Log("Collectable detected in area");
            var collectable = other.GetComponent<Collectable>();
            if (collectable != null) // Why would this even be null if we can compare tags? - because tags can change dynamically?
            {
                interact = Collect(collectable);   
            }
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            var nextRoomType = other.GetComponent<dpc>().roomType;
            interact = EnterDoor(nextRoomType);
        }
    }

    private Action EnterDoor(dpc.RoomType nextRoomType)
    {
        return () =>
        {
            if (Input.GetKey(KeyCode.E))
            {
                OnDoorInteraction?.Invoke(nextRoomType);
            }
        };
    }

    // deexpose collect function
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Leaving collectable zone");
        interact = () => { };
        
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
                Destroy(collectable.gameObject);
            }
        };
    }
}
