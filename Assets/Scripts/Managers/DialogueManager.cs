using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField]
    ///json that contains the compiled ink file
    private TextAsset _inkJson;
    /// <summary>
    /// Variable that contains the whole story interaction
    /// </summary>
    private Story _story;
    private bool _dialoguePlaying = false;
    [SerializeField]
    private InputActionAsset _playerControls;
    private InputAction _continueAction;
    private InputAction _exitAction;

    private void Awake()
    {
        //instantiate the story from the json
        _story = new Story(_inkJson.text);
        //use the Dialogue actions for the dialogue
        _continueAction = _playerControls.FindActionMap("Player").FindAction("Touch");
        _continueAction.performed += OnContinue;

        _exitAction = _playerControls.FindActionMap("Player").FindAction("Interact");
        _exitAction.performed += OnExit;
    }

    private void OnContinue(InputAction.CallbackContext context)
    {
        if (GameManager.i.State == StateMachineStep.Inspect)
            ContinueOrExitStory();
    }

    private void OnExit(InputAction.CallbackContext context)
    {
        //if (GameManager.i.State == StateMachineStep.Inspect)
        //    ExitDialogue();
    }

    private void OnEnable()
    {
        GameManager.i.DialogueEvents.OnEnterDialogue += EnterDialogue;
        GameManager.i.DialogueEvents.OnDialoguePanelClose += ExitDialogue;        
    }

    private void OnDisable()
    {
        GameManager.i.DialogueEvents.OnEnterDialogue -= EnterDialogue;
        GameManager.i.DialogueEvents.OnDialoguePanelClose -= ExitDialogue;
    }
    private void EnterDialogue(string knotName)
    {
        if (_dialoguePlaying)
            return;

        _dialoguePlaying = true;

        //inform all other systems that we started our interaction
        GameManager.i.DialogueEvents.DialogueStarted();

        if (!knotName.Equals(""))
        {
            //start the dialogue from the knot corresponding with the object
            _story.ChoosePathString(knotName);
        } else
        {
            Debug.LogWarning("Empty interaction dialogue");
        }

        //proceed with the dialogue
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (_story.canContinue)
        {
            string dialogueLine = _story.Continue();
            Debug.Log(dialogueLine);
            GameManager.i.DialogueEvents.DisplayDialogue(dialogueLine);   
        } else
        {
            ExitDialogue();
        }

    }

    private void ExitDialogue()
    {
        Debug.Log("Exiting dialogue");

        _dialoguePlaying = false;

        //inform all other systems that we finished our interaction
        GameManager.i.DialogueEvents.DialogueFinished();

        //reset the story to the start
        _story.ResetState();
    }
}
