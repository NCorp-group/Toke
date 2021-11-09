using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class slidertest : MonoBehaviour
{
    // Start is called before the first frame update

    private Slider _slider;
    public float value;
    void Start()
    {
        _slider = GetComponent<Slider>();
        //_slider.value = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        value = _slider.value;
    }
}
