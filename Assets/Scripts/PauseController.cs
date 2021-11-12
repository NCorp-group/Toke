using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private static bool _paused = false;

    // Start is called before the first frame update
    void Start()
    {
        //Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused) { Resume(); }
            else { Pause(); }
            _paused = !_paused;
        }
    }

    public void Resume()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
    }

    private void Pause()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
        _paused = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
