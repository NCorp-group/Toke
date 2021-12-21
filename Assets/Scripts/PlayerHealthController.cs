using UnityEngine;
using System;

public class PlayerHealthController : MonoBehaviour
{
    public static event Action<float, int> OnPlayerHealthChange;
    public static event Action OnPlayerDie;
    public static event Action OnPlayerTakeDamage;

    public int maxHealth = 100;
    public float currentHealth = 100;

    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PlayerPrefs.GetInt(MAX_HEALTH, maxHealth);
        currentHealth = PlayerPrefs.GetFloat(CURRENT_HEALTH, maxHealth);
        OnPlayerHealthChange?.Invoke(currentHealth, maxHealth);
        Debug.Log($"PP currentHealth = {PlayerPrefs.GetFloat(CURRENT_HEALTH, 0)}");
        Debug.Log($"PP maxHealth = {PlayerPrefs.GetFloat(MAX_HEALTH, 0)}");
    }

    // Update is called once per frame
    private void OnEnable()
    {
        Stats.OnMaxHealthChanged += HealthPickup;
    }

    private void OnDisable()
    {
        Stats.OnMaxHealthChanged += HealthPickup;
    }

    private const string MAX_HEALTH = "max_health";
    private const string CURRENT_HEALTH = "current_health";

    public void HealthPickup(int newMaxHealth)
    {
        Debug.Log($"PP currentHealth = {PlayerPrefs.GetFloat(CURRENT_HEALTH, 0)}");
        Debug.Log($"PP maxHealth = {PlayerPrefs.GetFloat(MAX_HEALTH, 0)}");
        if (!alive) return;

        var gainedHealth = Mathf.Abs(newMaxHealth - maxHealth);
        maxHealth = newMaxHealth;
        currentHealth += gainedHealth;
        OnPlayerHealthChange?.Invoke(currentHealth, newMaxHealth);
        //PlayerPrefs.SetFloat(CURRENT_HEALTH, currentHealth);
        Debug.Log($"PP currentHealth = {PlayerPrefs.GetFloat(CURRENT_HEALTH, 0)}");
        Debug.Log($"PP maxHealth = {PlayerPrefs.GetFloat(MAX_HEALTH, 0)}");
    }

    public void TakeDamage(float damage)
    {
        if (!alive) return;
        
        currentHealth -= damage;
        PlayerPrefs.SetFloat(CURRENT_HEALTH, currentHealth);

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
            alive = false;
        }
    }
}
