using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTakeDamageFlashSprite : MonoBehaviour
{
    #region Editor Settings
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flash;
    
    [Tooltip("Duration of the flash.")] 
    [SerializeField] private float duration;

    [SerializeField] private Color color;
    #endregion

    private SpriteRenderer _sr;
    private Material _original;
    private Material _flash;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _original = _sr.material;
        _flash = new Material(flash)
        {
            color = color
        };
    }

    public void Flash()
    {
        StartCoroutine((_Flash()));
    }

    private IEnumerator _Flash()
    {
        
        _sr.material = _flash;
        yield return new WaitForSeconds(duration);
        _sr.material = _original;
    }
}
