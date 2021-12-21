using System;
using UnityEngine;
using dpc = DoorPreviewController;

public class InteractableArea : MonoBehaviour
{
    public float interactableRadius = 0.6f;
    public LayerMask interactableMask;
    public static event Action<dpc.RoomType> OnDoorInteraction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.E))
        {
            var interactable = Physics2D.OverlapCircle(transform.position, interactableRadius, interactableMask);
            if (interactable is not null)
            {
                
                ItemStats statsToAdd = interactable.GetComponent<ItemStats>();
                var DPC = interactable.GetComponent<dpc>();
                var projectileItem = interactable.GetComponent<ProjectileItem>();
                var penningarBagDrop = interactable.GetComponent<PenningarDrop>();
                if (statsToAdd is not null) // Check if the Collectable has a specific component
                {
                    Stats tokeStats = GetComponentInParent<Stats>();

                    // Adding the rune stats to Toke
                    tokeStats.addPenningarAmount(statsToAdd.penningar);
                    tokeStats.addMaxHealth(statsToAdd.maxHealth);
                    tokeStats.addMovementSpeedScalar(statsToAdd.movementSpeed);
                    tokeStats.addLuckMultiplier(statsToAdd.luckMultiplier);
                    tokeStats.addFireRateScalar(statsToAdd.fireRateMultiplier);
                    tokeStats.addDamageMultiplier(statsToAdd.damageMultiplier);
                    tokeStats.addProjectileLifeMultiplier(statsToAdd.projectileLifeMultiplier);
                    tokeStats.addProjectilespeedMultiplier(statsToAdd.projectileSpeedMultiplier);

                    if (!statsToAdd.dropped)
                    {
                        tokeStats.addPenningarAmount(-statsToAdd.price);
                    }

                    Destroy(interactable.gameObject);
                }
                else if (DPC is not null)
                {
                    OnDoorInteraction?.Invoke(DPC.roomType);
                }
                else if(projectileItem is not null)
                {
                    projectileItem.CollectProjectile();
                    Destroy(interactable.gameObject);
                }
                else if(penningarBagDrop is not null)
                {
                    Stats tokeStats = GetComponentInParent<Stats>();
                    tokeStats.addPenningarAmount(penningarBagDrop.penningarsToCollect);
                    Destroy(interactable.gameObject);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactableRadius);
    }
}
