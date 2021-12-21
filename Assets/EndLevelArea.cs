using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelArea : MonoBehaviour
{
    public BifrostLight bifrostLight;
    public Collider2D area;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponentInParent<Stats>();
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        var movement = player.GetComponent<Movement>();
        movement.enabled = false;
        bifrostLight.CallBifrost(BifrostLight.Fade.In);
    }
}
