using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
    // singleton design pattern
    private static Player instance;
    public static Player GetInstance() => instance; 

    private void Awake()
    {
        // singleton design pattern
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    */
}
