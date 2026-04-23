using System.Collections;
using UnityEngine;
using UnityEngine.Playables;


public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _director;


    [SerializeField]
    private bool _playOnStart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_playOnStart)
            StartCountdown();

        //Debug.Log(_director.playableAsset.duration);
    }

    /// <summary>
    /// Starts the coroutine to invoke the change of state after the cutscene
    /// </summary>
    void StartCountdown()
    {
        StartCoroutine("ChangeState");
    }

    private IEnumerator ChangeState()
    {
        yield return new WaitForSeconds((float)(_director.playableAsset.duration+0.5f));
        GameManager.ChangeState(StateMachineStep.Free);
    }

    
}
