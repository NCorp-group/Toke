using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class FlashColorOnTakeDamage : MonoBehaviour
{
    public Color color;
    public float flashDuration = 0.5f;

    [Header("Must have a child object with tag = \"On Hit Flash Light\"")]
    public float flashIntensity = 1f;
    private Light2D _flash_light;

    public float ttr = 0.2f;
    public float ttf = 1.0f;
    
  
//    public Material mat;
 //   private Material orig_mat;

    
    private void Awake()
    {
//        orig_mat = GetComponent<SpriteRenderer>().material;
    }

    private Color _original_color;

    private SpriteRenderer sr;
    
    
    private bool lightUp = false;

    private float _min_flash_intensity;
    
    void Start()
    {
        Assert.IsTrue(ttf > 0 && ttr > 0);
        Assert.IsTrue(flashDuration > 0);
        sr = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(sr);
        _original_color = sr.material.color;
        // the modified shader
        //Assert.IsNotNull(mat);
        //mat.SetColor("_FlashColor", color);
        _flash_light = GetComponentsInChildren<Light2D>().FirstOrDefault(c => c.CompareTag("OnHitFlashLight"));
        Assert.IsNotNull(_flash_light);
        _flash_light.color = color;
        _min_flash_intensity = _flash_light.intensity;
    }

    private void OnEnable()
    {
        var enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnIndividualEnemyTakeDamage += OnOnIndividualEnemyTakeDamage;
        }

        var phc = GetComponent<PlayerHealthController>();
        if (phc != null)
        {
            PlayerHealthController.OnPlayerTakeDamage += FlashCB;
        }
    }

    private void OnDisable()
    {
        var enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnIndividualEnemyTakeDamage -= OnOnIndividualEnemyTakeDamage;
        }

        var phc = GetComponent<PlayerHealthController>();
        if (phc != null)
        {
            PlayerHealthController.OnPlayerTakeDamage -= FlashCB;
        }
    }

    private void OnOnIndividualEnemyTakeDamage()
    {
        //lightUp = true;
        //Debug.Log("INDIVIDUAL ENEMY HIT");
        Flash();
        // StartCoroutine(FlashColor());
    }

    private void Update()
    {
        // var slope = lightUp
        //     ? (flashIntensity - _min_flash_intensity) / 0.2f
        //     : (_min_flash_intensity - flashIntensity) / flashDuration; 
        //
        // var delta = _flash_light.intensity + _flash_light.intensity * Time.deltaTime * slope;
        // Debug.Log($"delta is {delta}");
        // _flash_light.intensity = Mathf.Clamp(delta, _min_flash_intensity, flashIntensity);
        //
        // if (Mathf.Approximately(_flash_light.intensity, flashIntensity) || _flash_light.intensity > flashIntensity)
        // {
        //     lightUp = false;
        // }
    }

    private void FlashCB()
    {
        //lightUp = true;
        Flash();
    }
    
    private IEnumerator flashCoroutine;
    
    private void Flash(){
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);
		
        flashCoroutine = DoLightUp();
        StartCoroutine(flashCoroutine);
    }

    private IEnumerator DoLightUp()
    {
        var rise_slope = (flashIntensity - 0.0f) / ttr;
        var fall_slope = (0.0f - flashIntensity) / ttf;
        
        float lerpTime = 0;
        while (!Mathf.Approximately(_flash_light.intensity, flashIntensity) || _flash_light.intensity > flashIntensity)
        {
            var delta = _flash_light.intensity + 1.0f * Time.deltaTime * rise_slope;
            Debug.Log($"delta: {delta}");
            _flash_light.intensity = Mathf.Clamp(delta, 0.0f, flashIntensity);
            yield return null;
        }
        
        while (!Mathf.Approximately(_flash_light.intensity, 0.0f) || _flash_light.intensity < 0.0f)
        {
            var delta = _flash_light.intensity + 1.0f * Time.deltaTime * fall_slope;
            Debug.Log($"delta: {delta}");
            _flash_light.intensity = Mathf.Clamp(delta, 0.0f, flashIntensity);
            yield return null;
        }

        
   
        
        // while (lerpTime < flashDuration)
        // {
        //     lerpTime += Time.deltaTime;
        //     float perc = lerpTime / flashDuration;
        //
        //     SetFlashAmount(flashIntensity - perc);
        //     yield return null;
        // }
        // SetFlashAmount(0);
    }
    
    // private IEnumerator DoFlash()
    // {
    //     float lerpTime = 0;
    //     GetComponent<SpriteRenderer>().material = mat;
    //
    //     while (lerpTime < flashDuration)
    //     {
    //         lerpTime += Time.deltaTime;
    //         float perc = lerpTime / flashDuration;
    //
    //         SetFlashAmount(1f - perc);
    //         yield return null;
    //     }
    //     GetComponent<SpriteRenderer>().material = orig_mat;
    //     SetFlashAmount(0);
    // }
	   //
    private void SetFlashAmount(float flashAmount)
    {
        _flash_light.intensity = flashAmount;
        // mat.SetFloat("_FlashAmount", flashAmount);
    }
}
