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
                RefreshGameManager();
            }

            return instance;
        }
    }

    public static void RefreshGameManager()
    {
        //retrieve the actions from the gameassets
        //instance.PlayerControls = GameAssets.i.PlayerControls;
        //event called every time the state machine changes state
        instance.OnChangeState = new UnityEvent<StateMachineStep, StateMachineStep>();

        if (!GameObject.FindGameObjectWithTag("CineMachine").TryGetComponent(out i.CinemachineController))
            Debug.LogWarning("No CineMachine controller found.");

        instance.Player = GameObject.FindGameObjectWithTag("Player");

        if (instance.Player == null)
        {
            Debug.LogWarning("No player found in scene.");
        }
        else
        {
            instance.Inventory = instance.Player.GetComponent<Inventory>();
            instance.PlayerController = instance.Player.GetComponent<PlayerController>();
        }

        GameManager.ChangeState(GameManager.i.State);

        instance.DialogueEvents = new DialogueEvents();
    }

    public StateMachineStep State { get; protected set; }
    public StateMachineStep PreviousState { get; protected set; }
    public CinemachineInputAxisController CinemachineController;
    //public CanvasManager CanvasManager;
    public DialogueEvents DialogueEvents;

    public GameObject Player;
    public Inventory Inventory;
    public PlayerController PlayerController;
    //public InventoryManager InventoryManager;
    //public InputActionAsset PlayerControls;

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

    /// <summary>
    /// Unregister an object from the list of handlers. Should be called OnDestroy of relative object that registered.
    /// </summary>
    /// <param name="id">Id of the item that should be removed</param>
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
        }
        else
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
        Debug.Log(itemId);
        Debug.Log(GameAssets.i.ItemDatabase);
        if (added)
        {
            instance.Inventory.AddToInventory(GameAssets.i.ItemDatabase.GetItem(itemId));
        }
        else
        {
            instance.Inventory.RemoveFromInventory(itemId);
        }
    }

    #endregion

    #region State machine
    public UnityEvent<StateMachineStep, StateMachineStep> OnChangeState;
    /// <summary>
    /// Used to handle all changes in the game state. Mostly to check what the player can do in the various states.
    /// </summary>
    /// <param name="newState">the new state it's entering</param>
    public static void ChangeState(StateMachineStep newState, GameObject from = null)
    {
        Debug.Log("Changed to " + newState + " from " + (from? from.name : "N/A"));
        i.PreviousState = i.State;
        i.State = newState;
        switch (newState)
        {
            case StateMachineStep.Free:
                instance.PlayerController.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                instance.CinemachineController.enabled = true;
                //switch on and off the correct action bindings
                //instance.PlayerControls.FindActionMap("Player").Enable();
                //instance.PlayerControls.FindActionMap("Dialogue").Disable();
                break;
            case StateMachineStep.Inventory:
            case StateMachineStep.Inspect:
                Cursor.lockState = CursorLockMode.None;
                instance.CinemachineController.enabled = false;
                //cute solution but I can't walk away from interactions so discarded
                //instance.PlayerControls.FindActionMap("Player").Disable();
                //instance.PlayerControls.FindActionMap("Dialogue").Enable();
                break;
            case StateMachineStep.Cutscene:
                instance.PlayerController.enabled = false;
                instance.CinemachineController.enabled = false;
                break;

        }
        i.OnChangeState.Invoke(newState, i.PreviousState);
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
    Inventory,
    Cutscene
}
