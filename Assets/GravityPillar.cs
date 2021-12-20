using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GravityPillar : MonoBehaviour
{
    public SpriteRenderer runes;
    public Light2D runesLight;
    public GravitySink gravitySink;

    [Range(0f, 30f)]
    public float maxIntensity;

    [Header("maxBodiesAfterWhichNoMoreWillResultInAVisibleChange")]
    [Range(0, 30)] public int maxBodies;

    public float ttr = 1f;
    public float ttf = 2f;
    
    private Color _color;

    private void Start()
    {

        _color = runes.color;
    }
    
    void Update()
    {
        // var proportion = maxIntensity / maxBodies;
        var bodies_affected = Math.Min(maxBodies, gravitySink.GetNumberOfBodiesAffected());
        var ratio = (float) bodies_affected / (float) maxBodies;
        _color.a = ratio;

        var tt = bodies_affected <= 0 ? ttf : ttr;
        var intensity = maxIntensity * tt * Time.deltaTime;
        
        runes.color = _color;
        runesLight.intensity = Mathf.Clamp(runesLight.intensity + intensity, 0, maxIntensity * ratio);
    }
}
