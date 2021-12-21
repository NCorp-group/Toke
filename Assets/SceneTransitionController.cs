using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [Header("If true show a transition when scene is loaded/started.")]
    public bool sceneStart = true;

    private Animator _animator;
    private static readonly int StartScene = Animator.StringToHash("StartScene");

    // Start is called before the first frame update
    void Awake()
    {
        if (sceneStart)
        {
            _animator = GetComponentInChildren<Animator>();
            _animator.SetTrigger(StartScene);
        }
    }
}
