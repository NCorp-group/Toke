using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class BifrostLight : MonoBehaviour
{
    private Light2D _light;
    private SpriteRenderer _sr;
    private Movement _movement;

    public CanvasGroup HUD;
    
    public float ttf = 2.5f;
    private float _t;
    
    public enum Fade
    {
        // dark to light
        In,
        // light to dark
        Out
    }

    public Fade fade = Fade.Out;

    public bool startOnSceneStart = true;
    
    private float _max_intensity;
    private float _max_outer_radius;

    private float maxIntensity = 30f;

    private float _max_falloff;
    // Start is called before the first frame update
    private void Start()
    {
        _light = GetComponent<Light2D>();
        /*
        var player = GameObject.FindWithTag("Player");
        _sr = player.GetComponent<SpriteRenderer>();
        _movement = player.GetComponent<Movement>();
        _movement.enabled = fade != Fade.Out;
        var color = _sr.color;
        color.a = 0;
        _sr.color = color;
        */
        //_light.lightType = Light2D.LightType.Point;
        _max_intensity = maxIntensity;
        //_max_falloff = _light.shapeLightFalloffSize;
        //_max_outer_radius = _light.pointLightOuterRadius;
    }

    public void CallBifrost(Fade f)
    {
        var player = GameObject.FindWithTag("Player");
        _sr = player.GetComponent<SpriteRenderer>();
        _movement = player.GetComponent<Movement>();
        _movement.enabled = fade != Fade.Out;
        StartCoroutine(_CallBifrost(f));
    }

    private IEnumerator _CallBifrost(Fade f)
    {
        _t = 0;

        var (start, end, start_intensity, end_intensity) =
            f == Fade.In 
                ? (1f, 0f, 0f, _max_intensity)
                : (0f, 1f, _max_intensity, 0f);

        while (_t < ttf)
        {
            _t += Time.deltaTime;
            var ratio = _t / ttf;
            _light.color = Color.HSVToRGB(Random.Range(0f, 1f), 0.2f, 1f);
            var color = _sr.color;
            color.a = Mathf.Lerp(start, end, ratio);
            _sr.color = color;
            _light.intensity = Mathf.Lerp(start_intensity, end_intensity, ratio);
            HUD.alpha = Mathf.Lerp(start, end, ratio);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // Destroy(gameObject);
    }

    // Update is called once per frame
    /*
    private void Update()
    {
        _t += Time.deltaTime;
        if (_t > ttf)
        {
            _movement.enabled = fade == Fade.Out;
            Destroy(gameObject);
        }

        var ratio = _t / ttf;

        _light.color = Color.HSVToRGB(Random.Range(0f, 1f), 0.2f, 1f);
        //Mathf.SmoothDamp();

        var color = _sr.color;
        color.a = Mathf.Lerp(0f, 1f, ratio);
        _sr.color = color;
        _light.intensity = Mathf.Lerp(_max_intensity, 0f, ratio);
        HUD.alpha = Mathf.Lerp(0f, 1f, ratio);
        //_light.falloffIntensity = Mathf.Lerp(_max_falloff, 0f, ratio);
        //_light.pointLightOuterRadius = Mathf.Lerp(_max_outer_radius, 0f, ratio);
    }
    */
}
