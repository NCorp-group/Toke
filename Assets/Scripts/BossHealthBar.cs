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

    public SpawnObjectsOnTargetEnter spte;
    private BossHealthController bhc;

    public CanvasRenderer healthbar;

    private void OnEnable()
    {
        spte.OnObjectsSpawned += OnObjectsSpawned;
    }

    private void OnObjectsSpawned()
    {
        bhc = FindObjectOfType<BossHealthController>();
        bhc.OnBossTakeDamage += UpdateHealthBar;
        bhc.OnBossDefeated += DisableHealthBar;
    }

    private void DisableHealthBar(BossHealthController.Boss obj)
    {
        healthbar.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    private void UpdateHealthBar(BossHealthController.Boss arg1, float arg2, float arg3)
    {
        var health_ratio = arg2 / arg3;
        Debug.Log($"helat ratio {health_ratio}");
        slider.value = health_ratio;
    }
    
    private void OnDisable()
    {
        bhc.OnBossTakeDamage -= UpdateHealthBar;
        bhc.OnBossDefeated -= DisableHealthBar;
    }

    void Start()
    {
        old_value = value;
        
    }

    // Update is called once per frame
    /*void Update()
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
    }*/
}
