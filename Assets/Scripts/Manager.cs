using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool _pauseActive;

    // Start is called before the first frame update
    void Start()
    {
        _pauseActive = false;
        pauseMenu.SetActive(_pauseActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            _pauseActive = !_pauseActive;
            pauseMenu.SetActive(_pauseActive);
        }
    }
}
