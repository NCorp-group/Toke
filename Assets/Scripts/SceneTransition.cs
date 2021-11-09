using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneID;

    public void LoadScene()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        SceneManager.LoadScene("Scenes/next_scene");
    }
}
