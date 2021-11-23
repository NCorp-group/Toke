using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class ControlLightIntensityOfCrystal : MonoBehaviour
{

    [SerializeField] private float idleIntensity = 1f;
    [SerializeField] private float lightUpIntensity = 4f;
    [SerializeField] private float timeToLightUpCompletely = 0.2f;
    [SerializeField] private float timeToReachIdle = 1f;
    
    //private Light2D _light;

    private Light2D[] _lights;
    
    private float _target;
    private bool _CR_running = false;

    private bool lightUp = false;

    private Func<float, float, (float, float)> handler;
    private IEnumerator coroutine;
    
    void Start()
    {
        // _light = GetComponent<Light2D>();

        _lights = GetComponentsInChildren<Light2D>();
        
        // Assert.IsNotNull(_light);
        Assert.IsTrue(lightUpIntensity > idleIntensity);
        _target = idleIntensity;
        handler = identity;
        
    }

    private void ChangeColorOfCrystal(Color color)
    {
        foreach (var light in _lights)
        {
            light.color = color;
        }
    }
    
    
    private void OnEnable()
    {
        // subscribe
        RangedWeapon.OnFire += IncreaseLightIntensity;
        RangedWeapon.OnProjectileChanged += ChangeColorOfCrystal;
        RangedWeapon.OnProjectileSetColor += ChangeColorOfCrystal;
    }

    private void OnDisable()
    {
        // unsubscribe
        RangedWeapon.OnFire -= IncreaseLightIntensity;
        RangedWeapon.OnProjectileChanged -= ChangeColorOfCrystal;
        RangedWeapon.OnProjectileSetColor -= ChangeColorOfCrystal;
    }

    
    
   

    private void IncreaseLightIntensity()
    {
        lightUp = true;
        
        /*
        if (_CR_running)
        {
            StopCoroutine(_IncreaseLightIntensity());
        }
        // FIXME: needs reference to coroutine
        StartCoroutine(_IncreaseLightIntensity());
        */
    }

    // Update is called once per frame
    void Update()
    {
        var slope = lightUp
            ? (lightUpIntensity - idleIntensity) / timeToLightUpCompletely
            : (idleIntensity - lightUpIntensity) / timeToReachIdle;

        var fstCrystal = _lights[0];
        var _light = fstCrystal;
        
        var delta = _light.intensity + _light.intensity * Time.deltaTime * slope;
        _light.intensity = Mathf.Clamp(delta, idleIntensity, lightUpIntensity);

        foreach (var light in _lights)   
        {
            light.intensity = Mathf.Clamp(delta, idleIntensity, lightUpIntensity);
        }
        
        /*
        Debug.Log($"light up? {lightUp}");
        var t += Time.deltaTime;
        _light.intensity = Mathf.Lerp(
            _light.intensity,
            lightUp ? lightUpIntensity : idleIntensity,
            t / (lightUp ? timeToLightUpCompletely : timeToReachIdle)
        );
        _light.intensity = Mathf.MoveTowards(_light.intensity, t,
            (lightUp ? timeToLightUpCompletely : timeToReachIdle) * Time.deltaTime * 150);
        */
        if (Mathf.Approximately(_light.intensity, lightUpIntensity) || _light.intensity > lightUpIntensity)
        {
            lightUp = false;
        }
        
        // Debug.Log($"light intensity = {_light.intensity}");
        
    }

    private IEnumerator _IncreaseLightIntensity()
    {
        _CR_running = true;
        _target = lightUpIntensity;
        yield return new WaitForSeconds(timeToLightUpCompletely);
        _target = idleIntensity;
        _CR_running = false;
    }

    private (float, float) identity(float a, float b) => (a, b);
    private (float, float) swap(float a, float b) => (b, a);

}
