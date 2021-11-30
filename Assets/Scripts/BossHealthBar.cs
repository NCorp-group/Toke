using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private const float TOLERANCE = Single.Epsilon;
    [Range(0,1)]
    [SerializeField] private float value = 1f;
    private float old_value;
    [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        old_value = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(value - old_value) > TOLERANCE)
        {
            old_value = value;
            var sliderValue = (int)(slider.maxValue * value);
            if (value > 0)
            {
                if (value >= 1)
                {
                    sliderValue = (int)slider.maxValue;
                }
                else if (sliderValue == 0)
                {
                    sliderValue = 1;
                }
            }

            Debug.Log("slidervalue = " + sliderValue);
            slider.value = sliderValue;
        }
    }
}
