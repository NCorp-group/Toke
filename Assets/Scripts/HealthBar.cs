using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float TOLERANCE = Single.Epsilon;
    [SerializeField] private Image healthBarSprite;
    [Range(0, 1)]
    [SerializeField] private float value = 1f;
    [SerializeField] private Sprite[] sprites;

    public int index;
    // Start is called before the first frame update
    void Start()
    {
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnEnable()
    {
        PlayerHealthController.OnPlayerHealthChange += UpdateValue;
    }

    void OnDisable()
    {
        PlayerHealthController.OnPlayerHealthChange -= UpdateValue;
    }

    private void UpdateValue(int currentHealth, int maxHealth)
    {
        Debug.Log(currentHealth);
        Debug.Log(maxHealth);
        value = (float) currentHealth / (float) maxHealth;
        Debug.Log(value);
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
