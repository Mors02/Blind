using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.Events;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Ink.Parsed;


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

                instance.Player = GameObject.FindGameObjectWithTag("Player");

                if (instance.Player == null) {
                    Debug.LogWarning("No player found in scene.");
                } else
                {
                    instance._inventory = instance.Player.GetComponent<Inventory>();
                }

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
    public DialogueEvents DialogueEvents;

    public GameObject Player;
    private Inventory _inventory;
    public InputActionAsset PlayerControls;

    #region Ink Functions
    /// <summary>
    /// Dictionary that holds all the possible objects that interact with ink
    /// </summary>
    private Dictionary<string, IActionHandler> _actionHandlers = new();

    /// <summary>
    /// Add an object to the list of handlers
    /// </summary>
    /// <param name="id">Id of the object that is added to the handler</param>
    /// <param name="handler">the handler of the object</param>
    public static void RegisterObject(string id, IActionHandler handler)
    {
        i._actionHandlers[id] = handler;
    }
    
    public static void UnregisterObject(string id)
    {
        i._actionHandlers.Remove(id);
    }

    /// <summary>
    /// Called by InkExternalFunctions, it's the link between ink and the game world
    /// </summary>
    /// <param name="objectId">Object the player is interacting with</param>
    /// <param name="actionId">Action on the object (optional based on the object)</param>
    public static void CompleteAction(string objectId, string actionId = "")
    {
        if (i._actionHandlers.TryGetValue(objectId, out var handler))
        {
            handler.Execute(actionId);
        } else
        {
            Debug.LogWarning("No handler registered for object: " + objectId);
        }
    }

    /// <summary>
    /// Update the inventory from ink
    /// </summary>
    /// <param name="itemId">item that should be added or removed</param>
    /// <param name="added">if true it's added, removed otherwise</param>
    public static void UpdateInventory(string itemId, bool added)
    {
        if (added)
        {
            instance._inventory.AddToInventory(GameAssets.i.ItemDatabase.GetItem(itemId));
        } else
        {
            instance._inventory.RemoveFromInventory(itemId);
        }
    }

    #endregion

    #region State machine
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
    #endregion
    

}

/// <summary>
/// describe what is the current situation of the state machine to know what the player can do
/// </summary>
public enum StateMachineStep
{
    Free,
    Inspect,
}
