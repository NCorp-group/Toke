using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static bool isPaused = false;
    private GameObject optionsMenu;
    private GameObject menuItems;
    /*private GameObject deathMenu;*/

    public Animator transitionAnimator;
    public float transitionDuration = 1f;

    private void OnEnable()
    {
        Stats.OnPlayerDie += ShowDeathMenu;
    }

    private void OnDisable()
    {
        Stats.OnPlayerDie -= ShowDeathMenu;
    }

    private void ShowDeathMenu()
    {
        //Debug.Log($"deathMenu = {deathMenu.name}");
        GetComponentsInChildren<Image>(true).First(
            o => o.name == "DeathMenu"
        ).gameObject.SetActive(true);
        StartCoroutine(FadeTime(10f));
    }

    private IEnumerator FadeTime(float fadeTime)
    {
        /*for (int i = 0; i <= STEPS; i++)
        {
            var ratio = (float) i / STEPS;
            Time.timeScale = Mathf.Lerp(1f, 0f, ratio);
            yield return new WaitForSecondsRealtime(5f / STEPS);
        }*/

        while (Time.timeScale > 0f)
        {
            var timeDiff = 1f * Time.fixedDeltaTime / fadeTime;
            Time.timeScale = Time.timeScale - timeDiff < 0f ? 0f : Time.timeScale - timeDiff;
            yield return null;
        }
    }

    private const int STEPS = 20;

    // Start is called before the first frame update
    private void Start()
    {
        optionsMenu = GetComponentsInChildren<OptionsMenu>(true).First().gameObject;
        
        menuItems = GetComponentsInChildren<Transform>(true).First(
            o => o.name == "MenuItems"
        ).gameObject;
        
        /*deathMenu = GetComponentsInChildren<Image>(true).First(
            o => o.name == "DeathMenu"
        ).gameObject;
        Debug.Log($"deathMenu = {deathMenu.name}");*/
        
        //Resume();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"deathMenu = {deathMenu.name}");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        AudioListener.pause = false;

        isPaused = false;
    }

    private void Pause()
    {
        if (optionsMenu.activeSelf) ToggleOptions();
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        AudioListener.pause = true;

        isPaused = true;
    }

    public void ToggleOptions()
    {
        //Debug.Log($"Toggling: {optionsMenu.activeSelf}");
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        menuItems.SetActive(!menuItems.activeSelf);
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
        Resume();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        // TODO: Save run progress here.
        Application.Quit();
    }

    public void Restart()
    {
        StartCoroutine(_Restart());
    }

    private IEnumerator _Restart()
    {
        transitionAnimator.SetTrigger(RoomManager.EndScene);
        yield return new WaitForSeconds(transitionDuration * 3);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
