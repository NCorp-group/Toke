using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;

    private Toggle masterToggle;
    private Toggle musicToggle;
    private Toggle sfxToggle;

    public static event Action<float, float, float> OnVolumeChanged;

    public static event Action<float> OnMasterVolumeChanged;
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSFXVolumeChanged;
    
    // Start is called before the first frame update
    void Start()
    {
        masterSlider = GetComponentsInChildren<Slider>().First(slider => slider.name == "MasterSlider");
        musicSlider = GetComponentsInChildren<Slider>().First(slider => slider.name == "MusicSlider");
        sfxSlider = GetComponentsInChildren<Slider>().First(slider => slider.name == "SFXSlider");
        
        masterToggle = GetComponentsInChildren<Toggle>().First(toggle => toggle.name == "MasterToggle");
        musicToggle = GetComponentsInChildren<Toggle>().First(toggle => toggle.name == "MusicToggle");
        sfxToggle = GetComponentsInChildren<Toggle>().First(toggle => toggle.name == "SFXToggle");
    }

    public void VolumeChange()
    {
        var valueMaster = masterSlider.value;
        var valueMusic = musicSlider.value;
        var valueSFX = sfxSlider.value;
        OnVolumeChanged?.Invoke(
            valueMaster, 
            valueMusic, 
            valueSFX
        );
    }

    /*
    public void MasterVolumeChange()
    {
        var value = masterSlider.value;
        OnMasterVolumeChanged?.Invoke(value);
    }

    public void MusicVolumeChange()
    {
        var value = musicSlider.value;
        OnMusicVolumeChanged?.Invoke(value);
    }

    public void SFXVolumeChange()
    {
        var value = sfxSlider.value;
        OnSFXVolumeChanged?.Invoke(value);
    }
    */

    public void VolumeToggle()
    {
        var masterToggled = masterToggle.isOn;
        var musicToggled = musicToggle.isOn;
        var sfxToggled = sfxToggle.isOn;

        OnVolumeChanged?.Invoke(
            masterToggled ? masterSlider.value : 0, 
            musicToggled ? musicSlider.value : 0, 
            sfxToggled ? sfxSlider.value : 0
        );
    }

    /*
    public void MasterVolumeToggle()
    {
        var toggled = masterToggle.isOn;
        OnMasterVolumeChanged?.Invoke(toggled ? 0 : masterSlider.value);
    }

    public void MusicVolumeToggle()
    {
        var toggled = musicToggle.isOn;
        OnMusicVolumeChanged?.Invoke(toggled ? 0 : musicSlider.value);
    }

    public void SFXVolumeToggle()
    {
        var toggled = sfxToggle.isOn;
        OnSFXVolumeChanged?.Invoke(toggled ? 0 : sfxSlider.value);
    }
    */
}
