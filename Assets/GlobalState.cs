using System;
using UnityEngine;

/// <summary>
/// Persist global state between scenes.
/// Unity has multiple ways of doing this, using static variables is
/// the easiest albeit not the most elegant ;)
/// </summary>
public class GlobalState : MonoBehaviour
{
    
    public static GlobalState instance;


    public static event Action OnSceneStart;
    
    public static event Action OnSceneEnd;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public static int playerHealth;
    // some other variables to keep track off
    public static Projectile projectile;

    public void LoadState()
    {
        
    }

    public void SaveState()
    {
        
    }

    private void Start()
    {
        OnSceneStart?.Invoke();
    }

    private void OnDestroy()
    {
        OnSceneEnd?.Invoke();
    }
}
