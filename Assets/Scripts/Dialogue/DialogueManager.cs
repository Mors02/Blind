using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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

    private InkExternalFunctions _inkExternalFunctions;
    private InkDialogueVariables _inkDialogueVariables;

    private int _currentChoiceIndex = -1;
    
    

    private void Awake()
    {
        //instantiate the story from the json
        _story = new Story(_inkJson.text);
        //use the Dialogue actions for the dialogue
        _continueAction = _playerControls.FindActionMap("Player").FindAction("Touch");
        _continueAction.performed += OnContinue;

        _exitAction = _playerControls.FindActionMap("Player").FindAction("Interact");
        _exitAction.performed += OnExit;

        _inkExternalFunctions = new InkExternalFunctions();
        _inkExternalFunctions.Bind(_story);

        _inkDialogueVariables = new InkDialogueVariables(_story);   
    }
#region On called events
    private void OnDestroy()
    {
        _inkExternalFunctions.Unbind(_story);
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
        //GameManager.i.CanvasManager.OnCloseCanvas.AddListener(ExternalExitDialogue);
        GameManager.i.DialogueEvents.OnDialoguePanelClose += ExternalExitDialogue;
        GameManager.i.DialogueEvents.OnChoiceSelected += UpdateChoiceIndex;
        GameManager.i.DialogueEvents.OnUpdateInkDialogueVariable += UpdateInkDialogueVariable;
    } 

    private void OnDisable()
    {
        GameManager.i.DialogueEvents.OnEnterDialogue -= EnterDialogue;
        //GameManager.i.CanvasManager.OnCloseCanvas.RemoveListener(ExternalExitDialogue);
        GameManager.i.DialogueEvents.OnDialoguePanelClose -= ExternalExitDialogue;
        GameManager.i.DialogueEvents.OnChoiceSelected -= UpdateChoiceIndex;
        GameManager.i.DialogueEvents.OnUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
    }
#endregion
    #region Dialogue handling
    private void EnterDialogue(string knotName)
    { 
        if (_dialoguePlaying)
            return;

        _dialoguePlaying = true;

        //inform all other systems that we started our interaction
        GameManager.i.DialogueEvents.DialogueStarted();

        //  _story.ResetState();
        
        if (!knotName.Equals(""))
        {
            //start the dialogue from the knot corresponding with the object
            _story.ChoosePathString(knotName);
        } else
        {
            Debug.LogWarning("Empty interaction dialogue");
        }

        //sync off variables with ink
        _inkDialogueVariables.SyncVariablesAndStartListening(_story);

        //proceed with the dialogue
        ContinueOrExitStory();
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this._currentChoiceIndex = choiceIndex;
        ContinueOrExitStory();
    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        _inkDialogueVariables.UpdateVariableState(name, value);
    }

    private void ContinueOrExitStory()
    {
        
        //if there are choices to make
        if (_story.currentChoices.Count > 0 && _currentChoiceIndex != -1)
        {
            //continue on our choice path
            _story.ChooseChoiceIndex(_currentChoiceIndex);
            //reset the current choice to avoid carry over
            _currentChoiceIndex = -1;
        }

        //otherwise if we can continue
        if (_story.canContinue)
        {
            //get next line (with eventual choices)
            string dialogueLine = _story.Continue();

            //skip over all the blank lines
            while (IsLineBlank(dialogueLine) && _story.canContinue)
            {
                dialogueLine = _story.Continue();
            }
            
            //if last line is empty then exit automatically
            if (IsLineBlank(dialogueLine) && !_story.canContinue)
            {
                ExitDialogue();
            } else
            {
                GameManager.i.DialogueEvents.DisplayDialogue(dialogueLine, _story.currentChoices);       
            }
            
        } 
        //exit only if there are no choices left
        else if (_story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }

    }

    private void ExitDialogue()
    {
        _dialoguePlaying = false;

        //inform all other systems that we finished our interaction
        GameManager.i.DialogueEvents.DialogueFinished();

        _inkDialogueVariables.StopListening(_story);

        //reset the story to the start
        _story.ResetState();
    }

    private void ExternalExitDialogue()
    {
         //Debug.Log("Exiting dialogue");

        _dialoguePlaying = false;
        //we do not want to inform other systems since this could cause an infinite loop

        _inkDialogueVariables.StopListening(_story);

        //reset the story to the start
        _story.ResetState();
    }

    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }

    #endregion
}
