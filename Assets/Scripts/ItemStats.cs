using UnityEngine;
using TMPro;

/* 
   This script gives an item the following three abilities:
   1. stats to give the player
   2. droppable functionality
   3. give the player a new projectile and staff lume
*/
public class ItemStats : MonoBehaviour
{
    private TextMeshProUGUI priceTag;
    [Space]
    [Header("Collectable stats")]
    // Stats:
    public int penningar = 0;
    public int maxHealth = 0;
    public float movementSpeed = 0;
    public float luckMultiplier = 0;
    public float fireRateMultiplier = 0;
    public float damageMultiplier = 0;
    public float projectileLifeMultiplier = 0;
    public float projectileSpeedMultiplier = 0;

    [Space]
    [Header("Shop Properties")]
    [SerializeField] bool dropped = true;
    [SerializeField] int price = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Sets the price text under the item
        if (!dropped)
        {
            priceTag = GetComponentInChildren<TextMeshProUGUI>();
            if (priceTag is not null)
            {
                priceTag.text = $"{price} P";
                priceTag.gameObject.SetActive(true);
            }
        }
    }
}
