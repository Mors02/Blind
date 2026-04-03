using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using Unity.Cinemachine;
using UnityEngine.InputSystem;


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
                //retrieve the actions from the gameassets
                instance.PlayerControls = GameAssets.i.PlayerControls;
                //event called every time the state machine changes state
                instance.OnChangeState = new UnityEvent<StateMachineStep>();
                
                if (!GameObject.FindGameObjectWithTag("CineMachine").TryGetComponent(out instance._cinemachineController))
                    Debug.LogWarning("No CineMachine controller found.");

                /*if (!GameObject.FindGameObjectWithTag("Canvas").TryGetComponent(out instance.CanvasManager))
                    Debug.LogWarning("No CanvasManager controller found.");*/

                GameManager.ChangeState(StateMachineStep.Free);

                instance.DialogueEvents = new DialogueEvents();
                
            }


            return instance;
        }
    }

    public StateMachineStep State {get; protected set;}
    private CinemachineInputAxisController _cinemachineController;
    //public CanvasManager CanvasManager;
    public UnityEvent<StateMachineStep> OnChangeState;
    public DialogueEvents DialogueEvents;

    public InputActionAsset PlayerControls;

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
                //switch on and off the correct action bindings
                //instance.PlayerControls.FindActionMap("Player").Enable();
                //instance.PlayerControls.FindActionMap("Dialogue").Disable();
                break;
            
            case StateMachineStep.Inspect:
                Cursor.lockState = CursorLockMode.None;
                instance._cinemachineController.enabled = false;
                //cute solution but I can't walk away from interactions so discarded
                //instance.PlayerControls.FindActionMap("Player").Disable();
                //instance.PlayerControls.FindActionMap("Dialogue").Enable();
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
