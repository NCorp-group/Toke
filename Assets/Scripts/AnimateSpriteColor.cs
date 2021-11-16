using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSpriteColor : MonoBehaviour
{
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Gradient fadein;
    [SerializeField] private Gradient fadeout;
    
    [SerializeField] private float time;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;
    
    private SpriteRenderer sr;
    private float t;
    private bool active;
    
    
    void Start()
    {
        sr.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t > time) t = 0.0f;

        sr.color = _gradient.Evaluate(time / t);
    }
}
