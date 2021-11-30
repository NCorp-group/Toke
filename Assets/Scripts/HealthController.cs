using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (maxHealth < 0)
        {
            maxHealth = 0;
        }
    }
}
