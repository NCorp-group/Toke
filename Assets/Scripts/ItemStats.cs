using UnityEngine;
using TMPro;

public class ItemStats : MonoBehaviour
{
    private TextMeshProUGUI priceTag;
    [Space]
    [Header("Stats")]
    // Stats:
    public int maxHealth = 0;
    public float movementSpeed = 0;
    public float luckMultiplier = 0;
    public float fireRate = 0;
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
        priceTag = GetComponentInChildren<TextMeshProUGUI>();
        if (!dropped)
        {
            priceTag.text = $"{price} P";   
        }
        else
        {
            priceTag.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
