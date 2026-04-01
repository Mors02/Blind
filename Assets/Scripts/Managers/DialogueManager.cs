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

    private void Awake()
    {
        //instantiate the story from the json
        _story = new Story(_inkJson.text);
        //use the Dialogue actions for the dialogue
        _continueAction = _playerControls.FindActionMap("Dialogue").FindAction("Continue");
        _continueAction.performed += OnContinue;
    }

    private void OnContinue(InputAction.CallbackContext context)
    {
        Debug.Log("Piero");
        ContinueOrExitStory();
    }

    private void OnEnable()
    {
        GameManager.i.DialogueEvents.OnEnterDialogue += EnterDialogue;        
    }

    private void OnDisable()
    {
        GameManager.i.DialogueEvents.OnEnterDialogue -= EnterDialogue;
    }
    private void EnterDialogue(string knotName)
    {
        if (_dialoguePlaying)
            return;

        _dialoguePlaying = true;

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
        } else
        {
            ExitDialogue();
        }

    }

    private void ExitDialogue()
    {
        Debug.Log("Exiting dialogue");

        _dialoguePlaying = false;

        //reset the story to the start
        _story.ResetState();
    }
}
