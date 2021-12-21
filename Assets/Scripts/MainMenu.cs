using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class MainMenu : MonoBehaviour
{
    public static event Action OnMainMenu;
    //public static event Action OnStartGame;

    public Animator transitionAnimator;
    public float transitionDuration = 1f;


    public void Start()
    {
        OnMainMenu?.Invoke();
    }

    public void StartGame()
    {
        StartCoroutine(_ToStoryController());
        //OnStartGame?.Invoke();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator _ToStoryController()
    {
        // TODO: Save run progress here.
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger(RoomManager.EndScene);
        yield return new WaitForSeconds(transitionDuration * 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
