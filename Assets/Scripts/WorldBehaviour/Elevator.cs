using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour, IActionHandler
{

    [SerializeField]
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
    }

    private IEnumerator StartTransition()
    {
        //Wait for the change state to the inspect of the end of dialogue to be first
        yield return new WaitForEndOfFrame();
        GameManager.ChangeState(StateMachineStep.Cutscene, gameObject);

        //then wait for the transition
        yield return new WaitForSeconds(_transitionDuration);
        SceneManager.LoadScene("Level2");
    }
}
