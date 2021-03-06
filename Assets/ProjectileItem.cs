using System;
using TMPro;
using UnityEngine;

public class ProjectileItem : MonoBehaviour
{
    public string projectileType = Projectile.Type.BOLT;
    public int price = 0;
    public float fireRate = 1;
    public Color color = Color.black;

    public static event Action<string> OnProjectileCollected;
    public static event Action<float> OnFireRateChanged;
    public static event Action<Color> OnProjectileSetColor;
    private TextMeshProUGUI priceTag;

    private void Start()
    {
        priceTag = GetComponentInChildren<TextMeshProUGUI>(true);
        Debug.Log($"{priceTag.text}");
        if (priceTag is not null)
        {
            priceTag.text = $"{price} P";
            priceTag.enabled = true;
        }
    }

    public void CollectProjectile()
    {
        //Debug.Log($"Invoking with {projectileType}");
        OnProjectileCollected?.Invoke(projectileType);
        OnFireRateChanged?.Invoke(fireRate);
        OnProjectileSetColor(color);
        //RangedWeapon.OnProjectileSetColor?.Invoke(color);
    }
    // Start is called before the first frame update
    /*void Start()
    {
        // Sets the price text under the item
        priceTag = GetComponentInChildren<TextMeshProUGUI>();
        if (!dropped)
        {
            priceTag.text = $"{price} P";
        }
        else
        {
            priceTag.text = "";
        }
    }*/
}
