using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float TOLERANCE = Single.Epsilon;
    [SerializeField] private Image healthBarSprite;
    [Range(0, 1)]
    [SerializeField] private float value;
    private float oldValue;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject character;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        value = 1f;
        oldValue = value;
        UpdateSprite();
        //hpSprite = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthController toke = character.GetComponent<HealthController>();
        value = (float) toke.currentHealth / (float) toke.maxHealth;
        
        //var
        if (Math.Abs(value - oldValue) > TOLERANCE)
        {
            if (value > 1)
            {
                value = 1;
            }
            else if (value < 0)
            {
                value = 0;
            }
            UpdateSprite();
            oldValue = value;
        }
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

            if (value < 1 && index == sprites.Length)
            {
                index -= 1;
            }
            //index = 1;
            healthBarSprite.sprite = sprites[index];
        }
    }
}
