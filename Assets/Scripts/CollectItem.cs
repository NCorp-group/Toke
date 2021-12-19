using System;
using UnityEngine;
using dpc = DoorPreviewController;

public class CollectItem : MonoBehaviour
{
    private Action interact;
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
    }

    // expose collect function
    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    // deexpose collect function
    private void OnTriggerExit2D(Collider2D other)
    {

    }
}