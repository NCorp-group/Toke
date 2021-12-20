using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static bool isPaused = false;
    private GameObject optionsMenu;
    // Start is called before the first frame update
    void Start()
    {
        optionsMenu = GetComponentsInChildren<OptionsMenu>(true).First().gameObject;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
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
        Time.timeScale = 0.1f;
        pauseMenu.SetActive(true);
        AudioListener.pause = true;

        isPaused = true;
    }

    public void ToggleOptions()
    {
        Debug.Log($"Toggling: {optionsMenu.activeSelf}");
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        
    }

    public void ToMainMenu()
    {
        // TODO: Save run progress here.
        Resume();
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        // TODO: Save run progress here.
        Application.Quit();
    }
}
