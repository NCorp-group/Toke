using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System;

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

    public static event Action OnBifrost;

    public Fade fade = Fade.Out;

    public bool startOnSceneStart = true;

    public bool isLastScene = false;
    public Animator transitionAnimator;
    public float transitionDuration = 1f;

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
        if (startOnSceneStart)
        {
            CallBifrost(fade);
        }
        
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

        OnBifrost?.Invoke();

        while (_t < ttf)
        {
            _t += Time.deltaTime;
            var ratio = _t / ttf;
            _light.color = Color.HSVToRGB(UnityEngine.Random.Range(0f, 1f), 0.2f, 1f);
            var color = _sr.color;
            color.a = Mathf.Lerp(start, end, ratio);
            _sr.color = color;
            _light.intensity = Mathf.Lerp(start_intensity, end_intensity, ratio);
            HUD.alpha = Mathf.Lerp(start, end, ratio);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        _movement.enabled = fade != Fade.In;

        if (isLastScene)
        {
            StartCoroutine(_ToEndStory());
        }
        // Destroy(gameObject);
    }

    private IEnumerator _ToEndStory()
    {
        // TODO: Save run progress here.
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger(RoomManager.EndScene);
        yield return new WaitForSeconds(transitionDuration * 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
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
