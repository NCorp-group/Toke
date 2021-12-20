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

    private float _max_intensity;
    private float _max_outer_radius;

    private float _max_falloff;
    // Start is called before the first frame update
    private void Start()
    {
        _light = GetComponent<Light2D>();
        var player = GameObject.FindWithTag("Player");
        _sr = player.GetComponent<SpriteRenderer>();
        _movement = player.GetComponent<Movement>();
        _movement.enabled = false;
        var color = _sr.color;
        color.a = 0;
        _sr.color = color;
        //_light.lightType = Light2D.LightType.Point;
        _max_intensity = _light.intensity;
        //_max_falloff = _light.shapeLightFalloffSize;
        //_max_outer_radius = _light.pointLightOuterRadius;
    }

    // Update is called once per frame
    private void Update()
    {
        _t += Time.deltaTime;
        if (_t > ttf)
        {
            _movement.enabled = true;
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
}
