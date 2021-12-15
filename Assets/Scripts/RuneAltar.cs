using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class RuneAltar : MonoBehaviour
{
    private Collider2D _collider_area;
    private Light2D _runes;

    [SerializeField] private Color runeColor;
    [SerializeField] private float minIntensity, maxIntensity;
    [Header("Time To Rise")]
    [SerializeField] private float ttr;
    [Header("Time To Fall")]
    [SerializeField] private float ttf;

    private bool _player_inside = false;
    private float _intensity_target;

    private Func<float, float, (float, float)> handler;
    private (float, float) identity(float a, float b) => (a, b);
    private (float, float) swap(float a, float b) => (b, a);

    
    private float t;
    
    // Start is called before the first frame update
    private void Start()
    {
        _collider_area = GetComponent<Collider2D>();
        Assert.IsNotNull(_collider_area);
        _runes = GetComponentInChildren<Light2D>();
        Assert.IsNotNull(_runes);
        Assert.IsTrue(minIntensity >= 0 && maxIntensity > minIntensity);
        Assert.IsTrue(ttr > 0 && ttf > 0);
        _intensity_target = minIntensity;
        handler = ((a, b) => (0, 0));
        t = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        t += Time.deltaTime;
        var (a, b) = handler(maxIntensity, minIntensity);
        _runes.intensity = Mathf.Clamp(
            Mathf.Lerp(a, b, ((_player_inside ? ttr : ttf) - t) / (_player_inside ? ttr : ttf)),
            minIntensity,
            maxIntensity
            );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player_inside = true;
            _intensity_target = maxIntensity;
            handler = identity;
            t = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player_inside = false;
            _intensity_target = minIntensity;
            handler = swap;
            t = 0;
        }
    }
}
