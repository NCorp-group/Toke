using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public static event Action OnMainMenu;
    public static event Action OnStartGame;


    public void Start()
    {
        OnMainMenu?.Invoke();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        OnStartGame?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
