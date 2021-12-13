using UnityEngine;
using System;

public class PlayerHealthController : MonoBehaviour
{

    public static event Action<int, int> OnPlayerHealthChange;
    public static event Action OnPlayerDie;
    public static event Action OnPlayerTakeDamage;

    public int maxHealth = 100;
    public int currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void HealthPickup(int pickupAmount)
    {
        maxHealth += pickupAmount;
        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.LogWarning(currentHealth);

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);

        if (currentHealth > 0)
        {
            OnPlayerTakeDamage?.Invoke();
        }
        else
        {
            OnPlayerDie?.Invoke();
        }

       
    }
}
