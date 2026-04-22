using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour, IActionHandler
{

    private float _transitionDuration;
    private void Start()
    {
        GameManager.RegisterObject("elevator", this);
    }

    private void OnDestroy()
    {
        GameManager.UnregisterObject("elevator");
    }

    public void Execute(string actionId)
    {
        StartCoroutine("StartTransition");
        GameManager.ChangeState(StateMachineStep.Cutscene);
    }

    private IEnumerator StartTransition()
    {
        yield return new WaitForSeconds(_transitionDuration);
        SceneManager.LoadScene("Level2");
    }
}
