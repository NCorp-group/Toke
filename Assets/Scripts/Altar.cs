using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Altar : MonoBehaviour
{
    public SpriteRenderer[] runes;

    public string gotoSceneName;
    
    /*
    public byte alphaStart = 0;
    public byte alpha_end = 255;
    public byte alpha_per_tick = 32;
    
    public float animation_speed = 1f;
    */
    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var rune in runes)
        {
            var opaque = MakeOpaque(rune.color);
            rune.color = opaque;
        }

        if (gotoSceneName != null)
        {
            //SceneManager.LoadScene("Scenes/next_scene");
            SceneManager.LoadScene(gotoSceneName);
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (var rune in runes)
        {
            var transparent = MakeTransparent(rune.color);
            rune.color = transparent;
        }   
    }

    private static Color MakeOpaque(Color color)
    {
        return SetAlphaOfColor(color, 1f);
    }

    private static Color MakeTransparent(Color color)
    {
        return SetAlphaOfColor(color, 0f);
    }
    
    private static Color SetAlphaOfColor(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
