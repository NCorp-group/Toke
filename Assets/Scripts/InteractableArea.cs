using System.Collections;
using System.Collections.Generic;
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var interactable = Physics2D.OverlapCircle(transform.position, interactableRadius, interactableMask);
            if (interactable is not null)
            {
                ItemStats statsToAdd = interactable.GetComponent<ItemStats>();
                var DPC = interactable.GetComponent<dpc>();
                if (statsToAdd != null) // Check if the Collectable has a specific component
                {
                    Stats tokeStats = GetComponentInParent<Stats>();

                    // Adding the rune stats to Toke
                    tokeStats.addMaxHealthChanged(statsToAdd.maxHealth);
                    tokeStats.addMovementSpeedScalarChanged(statsToAdd.movementSpeed);
                    tokeStats.addLuckMultiplierChanged(statsToAdd.luckMultiplier);
                    tokeStats.addFireRateScalarChanged(statsToAdd.fireRate);
                    tokeStats.addOnDamageMultiplierChanged(statsToAdd.damageMultiplier);
                    tokeStats.addProjectileLifeMultiplier(statsToAdd.projectileLifeMultiplier);
                    tokeStats.addOnProjectilespeedMultiplierChanged(statsToAdd.projectileSpeedMultiplier);
                    Destroy(interactable.gameObject);
                }
                else if (DPC is not null)
                {
                    OnDoorInteraction?.Invoke(DPC.roomType);
                }
            }

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactableRadius);
    }
}
