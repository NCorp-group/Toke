using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public static bool isPaused = false;
    private GameObject optionsMenu;
    private GameObject menuItems;

    public Animator transitionAnimator;
    public float transitionDuration = 1f;
    // Start is called before the first frame update
    void Start()
    {
        optionsMenu = GetComponentsInChildren<OptionsMenu>(true).First().gameObject;
        //Debug.Log($"HELLO");
        menuItems = GetComponentsInChildren<Transform>(true).First(o => o.name == "MenuItems").gameObject;
        var allChildren = GetComponentsInChildren<GameObject>(true);
        //Debug.Log($"Amount of children {allChildren.Length}");
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
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        // TODO: Save run progress here.
        Application.Quit();
    }
}
