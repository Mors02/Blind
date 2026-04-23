using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour, IActionHandler
{

    [SerializeField]
    private float _transitionDuration;

    [SerializeField]
    private StudioEventEmitter _eventEmitter;
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

        _eventEmitter.Stop();
        AudioManager.Instance.PlayOneShot(GameAssets.i.InteractionSounds.GetSound("ElevatorDoorClose"));

        //yield return new WaitForSeconds(2f);
        //AudioManager.Instance.PlayOneShot(GameAssets.i.InteractionSounds.GetSound("ElevatorRideLoop"));

        //then wait for the transition
        yield return new WaitForSeconds(_transitionDuration);
        SceneManager.LoadScene("Level2");
    }
}
