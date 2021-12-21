using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class StoryController : MonoBehaviour
{
    public static event Action OnStartGame;

    public Animator transitionAnimator;
    public float transitionDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Continue()
    {
        StartCoroutine(_ToFirstRoom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator _ToFirstRoom()
    {
        // TODO: Save run progress here.
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger(RoomManager.EndScene);
        yield return new WaitForSeconds(transitionDuration * 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        OnStartGame?.Invoke();
    }

    public void ToMainMenu()
    {
        StartCoroutine(_ToMainMenu());
    }

    private IEnumerator _ToMainMenu()
    {
        // TODO: Save run progress here.
        Time.timeScale = 1f;
        transitionAnimator.SetTrigger(RoomManager.EndScene);
        yield return new WaitForSeconds(transitionDuration * 3);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
