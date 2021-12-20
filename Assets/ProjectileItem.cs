using System;
using UnityEngine;

public class ProjectileItem : MonoBehaviour
{
    public string projectileType = Projectile.Type.BOLT;
    public int price = 0;

    public static event Action<string> OnProjectileCollected;

    public void CollectProjectile()
    {
        //Debug.Log($"Invoking with {projectileType}");
        OnProjectileCollected?.Invoke(projectileType);
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
