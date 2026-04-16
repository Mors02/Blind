using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private AnimationClip _startClip;
    
    public void Start()
    {
        if (!gameObject.TryGetComponent(out _animator))
        {
            Debug.LogWarning("No animator on " + name);
        }
    }

    public void Exit()
    {
        Debug.LogWarning("Only works with built game.");
        Application.Quit();
    }

    public void StartGame()
    {
        _animator.SetTrigger("Start");
        StartCoroutine("ChangeScene");
    }

    public IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(_startClip.length - 0.02f);
        SceneManager.LoadScene("Level1");
    }
}
