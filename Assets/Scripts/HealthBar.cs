using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarSprite;
    [SerializeField] private float value;
    private float oldValue;
    [SerializeField] private Sprite[] sprites;
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
        if (value != oldValue)
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
            var index = (int) (value * 48) - 1;
            if (index < 1)
            {
                index = 1;
            }
            Debug.Log(index);
            healthBarSprite.sprite = sprites[index];
        }
    }
}
