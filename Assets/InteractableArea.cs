using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableArea : MonoBehaviour
{
    public float interactableRadius = 0.6f;
    public LayerMask interactableMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var interactableList = Physics2D.OverlapCircleAll(transform.position, interactableRadius, interactableMask);
            if (interactableList.Length > 0)
            {
                Collider2D firstInteractable = interactableList[0];
                ItemStats statsToAdd = firstInteractable.GetComponent<ItemStats>();
                if (statsToAdd != null) // Check if the Collectable has a specific component.
                {
                    Stats tokeStats = GetComponentInParent<Stats>();

                    // Adding the rune stats to Toke
                    if (statsToAdd.maxHealth != 0) tokeStats.addMaxHealthChanged(statsToAdd.maxHealth);
                    if (statsToAdd.movementSpeed != 0) tokeStats.addMovementSpeedScalarChanged(statsToAdd.movementSpeed);
                    if (statsToAdd.luckMultiplier != 0) tokeStats.addLuckMultiplierChanged(statsToAdd.luckMultiplier);
                    if (statsToAdd.fireRate != 0) tokeStats.addFireRateScalarChanged(statsToAdd.fireRate);
                    if (statsToAdd.damageMultiplier != 0) tokeStats.addOnDamageMultiplierChanged(statsToAdd.damageMultiplier);
                    if (statsToAdd.projectileLifeMultiplier != 0) tokeStats.addProjectileLifeMultiplier(statsToAdd.projectileLifeMultiplier);
                    if (statsToAdd.projectileSpeedMultiplier != 0) tokeStats.addOnProjectilespeedMultiplierChanged(statsToAdd.projectileSpeedMultiplier);
                }
                Destroy(firstInteractable.gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactableRadius);
    }
}
