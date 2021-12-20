using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float TOLERANCE = Single.Epsilon;
    [SerializeField] private Image healthBarSprite;
    [Range(0, 1)]
    [SerializeField] private float value = 1f;
    [SerializeField] private Sprite[] sprites;

    private TextMeshProUGUI currentHealth;
    private TextMeshProUGUI maxHealth;
        
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = GetComponentsInChildren<TextMeshProUGUI>().First(tmp => tmp.name == "CurrentHealthText");
        maxHealth = GetComponentsInChildren<TextMeshProUGUI>().First(tmp => tmp.name == "MaxHealthText");
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        PlayerHealthController.OnPlayerHealthChange += UpdateValue;
        PlayerHealthController.OnPlayerHealthChange += UpdateText;
    }

    void OnDisable()
    {
        PlayerHealthController.OnPlayerHealthChange -= UpdateValue;
        PlayerHealthController.OnPlayerHealthChange += UpdateText;
    }

    private void UpdateText(float currentHealth, int maxHealth)
    {
        if (this.currentHealth is not null) this.currentHealth.text = $"{(int) currentHealth}";
        if (this.maxHealth is not null) this.maxHealth.text = $"{maxHealth}";
    }

    private void UpdateValue(float currentHealth, int maxHealth)
    {
        //Debug.Log(currentHealth);
        //Debug.Log(maxHealth);
        value = currentHealth / maxHealth;
        //Debug.Log(value);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (value <= 0)
        {
            healthBarSprite.sprite = sprites[0];
        }
        else
        {
            var index = (int) (value * sprites.Length) - 1;
            this.index = index;
            if (index < 1)
            {
                index = 1;
            }

            if (value < 1 && (index + 1) == sprites.Length)
            {
                index -= 1;
            }
            healthBarSprite.sprite = sprites[index];
        }
    }
}
