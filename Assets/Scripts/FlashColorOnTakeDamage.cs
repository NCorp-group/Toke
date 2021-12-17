using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;

public class FlashColorOnTakeDamage : MonoBehaviour
{
    public Color color;
    public float flashDuration = 0.5f;

    [Header("Must have a child object with tag = \"On Hit Flash Light\"")]
    public float flashIntensity = 1f;
    private Light2D _flash_light;

    public float ttr = 0.2f;
    public float ttf = 1.0f;

    private Color _original_color;
    //private SpriteRenderer sr;
    private bool lightUp = false;
    private float _min_flash_intensity;
    
    void Start()
    {
        Assert.IsTrue(ttf > 0 && ttr > 0);
        Assert.IsTrue(flashDuration > 0);
        //sr = GetComponent<SpriteRenderer>();
        //Assert.IsNotNull(sr);
        //_original_color = sr.material.color;
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

    private void OnOnIndividualEnemyTakeDamage() => Flash();
    private void FlashCB() => Flash();
    
    
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
            // Debug.Log($"delta: {delta}");
            _flash_light.intensity = Mathf.Clamp(delta, 0.0f, flashIntensity);
            yield return null;
        }
        
        while (!Mathf.Approximately(_flash_light.intensity, 0.0f) || _flash_light.intensity < 0.0f)
        {
            var delta = _flash_light.intensity + 1.0f * Time.deltaTime * fall_slope;
            // Debug.Log($"delta: {delta}");
            _flash_light.intensity = Mathf.Clamp(delta, 0.0f, flashIntensity);
            yield return null;
        }
    }

    private void SetFlashAmount(float flashAmount)
    {
        _flash_light.intensity = flashAmount;
    }
}
