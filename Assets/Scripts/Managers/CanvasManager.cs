using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private DialogueChoiceButton[] _choiceButtons;

    [SerializeField]
    private TextFadeTransition _textFade;

    [SerializeField]
    private InputActionAsset _playerControls;

    private InputAction _exitAction;

    [SerializeField]
    private GameObject _exitMenuSection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*Debug.Log(GameManager.i.State);
        if (GameManager.i.State == StateMachineStep.Free)
            this.gameObject.SetActive(false);*/

        StateChanged(GameManager.i.State, StateMachineStep.Free);

        GameManager.i.DialogueEvents.OnDisplayDialogue += DisplayText;

        GameManager.i.DialogueEvents.OnDialogueFinished += CloseInspectMenu;

        GameManager.i.OnChangeState.AddListener(StateChanged);

        _exitAction = _playerControls.FindActionMap("Player").FindAction("Exit");
        _exitAction.performed += ChangeStateExitMenu;

        _exitMenuSection.SetActive(false);

        //OnCloseCanvas.AddListener(Test);
    }

    public void DisplayText(string dialogueLine, List<Choice> choices)
    {
        //_text.text = dialogueLine;
        if (_textFade.IsFirstText())
            _textFade.FadeIn(dialogueLine);
        else
            _textFade.TransitionTo(dialogueLine);

        if (choices.Count > _choiceButtons.Length)
        {
            Debug.LogWarning("You put too many options (Max 6), currently " + choices.Count);
        }

        //Reset the state of all the buttons
        foreach (DialogueChoiceButton choiceButton in _choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i];
            DialogueChoiceButton button = _choiceButtons[i];

            button.gameObject.SetActive(true);
            button.SetChoiceText(choice.text);
            button.SetChoiceIndex(choice.index);

            /*if (i == 0)
            {
                button.SelectButton();
                GameManager.i.DialogueEvents.UpdateChoiceIndex(0);
            }*/
        }

    }

    private void ChangeStateExitMenu(InputAction.CallbackContext context)
    {

        if (_exitMenuSection.activeSelf) {
            GameManager.ChangeState(StateMachineStep.Free);
            _exitMenuSection.SetActive(false);
        }
        else {
            _exitMenuSection.SetActive(true);
            GameManager.ChangeState(StateMachineStep.Inspect);
        }
            
    }

    private void ResetPanel()
    {
        _text.text = "";
    }

    public void StateChanged(StateMachineStep newState, StateMachineStep oldState)
    {
        Debug.Log(oldState + " -> " + newState);
        switch (newState)
        {
            case StateMachineStep.Cutscene:
            case StateMachineStep.Free:
                ResetTriggers();
                _animator.SetTrigger("Hide");
                if (oldState == StateMachineStep.Inspect)
                    _textFade.FadeOut();
                ResetPanel();
                GameManager.i.DialogueEvents.CloseDialoguePanel();

                //Reset the state of all the buttons
                foreach (DialogueChoiceButton choiceButton in _choiceButtons)
                {
                    choiceButton.gameObject.SetActive(false);
                }
                break;
            case StateMachineStep.Inventory:
                if (oldState == StateMachineStep.Free)
                {
                    ResetTriggers();
                    _animator.SetTrigger("Show");
                }

                _textFade.FadeOut();

                //Reset the state of all the buttons
                foreach (DialogueChoiceButton choiceButton in _choiceButtons)
                {
                    choiceButton.gameObject.SetActive(false);
                }
                break;
            case StateMachineStep.Inspect:
                if (oldState == StateMachineStep.Free)
                {
                    ResetTriggers();
                    _animator.SetTrigger("Show");
                }
                break;

        }   
    }

    public void CloseInspectMenu()
    {
        GameManager.ChangeState(GameManager.i.PreviousState, gameObject);
    }

    public void CloseGame()
    {
        Debug.LogWarning("Only works with built game.");
        Application.Quit();
    }

    private void ResetTriggers()
    {
        _animator.ResetTrigger("Show");
        _animator.ResetTrigger("Hide");
    }
}
