using UnityEngine;
using System;

public class PlayerHealthController : MonoBehaviour
{

    public static event Action<float, int> OnPlayerHealthChange;
    public static event Action OnPlayerDie;
    public static event Action OnPlayerTakeDamage;

    public int maxHealth = 100;
    public float currentHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
    }

    // Update is called once per frame
    private void OnEnable()
    {
        Stats.OnMaxHealthChanged += HealthPickup;
    }

    public void HealthPickup(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        OnPlayerHealthChange?.Invoke(currentHealth, newMaxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.LogWarning(currentHealth);

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
