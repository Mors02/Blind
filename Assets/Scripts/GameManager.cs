using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using Unity.Cinemachine;

public class GameManager
{
    private static GameManager instance;
    public static GameManager i
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.OnChangeState = new UnityEvent<StateMachineStep>();
                if (!GameObject.FindGameObjectWithTag("CineMachine").TryGetComponent(out instance._cinemachineController))
                    Debug.LogWarning("No CineMachine controller found.");

                GameManager.ChangeState(StateMachineStep.Free);
                
            }

            return instance;
        }
    }

    public StateMachineStep State {get; protected set;}

    private CinemachineInputAxisController _cinemachineController;

    public UnityEvent<StateMachineStep> OnChangeState;

    /// <summary>
    /// Used to handle all changes in the game state. Mostly to check what the player can do in the various states.
    /// </summary>
    /// <param name="newState">the new state it's entering</param>
    public static void ChangeState(StateMachineStep newState)
    {
        
        i.State = newState;
        switch(newState)
        {
            case StateMachineStep.Free:
                Cursor.lockState = CursorLockMode.Locked;
                instance._cinemachineController.enabled = true;
                break;
            
            case StateMachineStep.Inspect:
                Cursor.lockState = CursorLockMode.None;
                instance._cinemachineController.enabled = false;
                break;
        }

        i.OnChangeState.Invoke(newState);
    }

}

/// <summary>
/// describe what is the current situation of the state machine to know what the player can do
/// </summary>
public enum StateMachineStep
{
    Free,
    Inspect,
}
