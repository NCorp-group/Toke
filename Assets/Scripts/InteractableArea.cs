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
            Debug.Log("Pressed E");
            var interactable = Physics2D.OverlapCircle(transform.position, interactableRadius, interactableMask);
            if (interactable is not null)
            {
                
                ItemStats statsToAdd = interactable.GetComponent<ItemStats>();
                var DPC = interactable.GetComponent<dpc>();
                var projectileItem = interactable.GetComponent<ProjectileItem>();
                if (statsToAdd is not null) // Check if the Collectable has a specific component
                {
                    Stats tokeStats = GetComponentInParent<Stats>();

                    // Adding the rune stats to Toke
                    tokeStats.addMaxHealth(statsToAdd.maxHealth);
                    tokeStats.addMovementSpeedScalar(statsToAdd.movementSpeed);
                    tokeStats.addLuckMultiplier(statsToAdd.luckMultiplier);
                    tokeStats.addFireRateScalar(statsToAdd.fireRateMultiplier);
                    tokeStats.addDamageMultiplier(statsToAdd.damageMultiplier);
                    tokeStats.addProjectileLifeMultiplier(statsToAdd.projectileLifeMultiplier);
                    tokeStats.addProjectilespeedMultiplier(statsToAdd.projectileSpeedMultiplier);
                    Destroy(interactable.gameObject);
                }
                else if (DPC is not null)
                {
                    Debug.Log("DPC FOUND POG");
                    OnDoorInteraction?.Invoke(DPC.roomType);
                }
                else if(projectileItem is not null)
                {
                    projectileItem.CollectProjectile();
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
