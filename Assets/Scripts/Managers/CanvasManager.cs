using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        /*Debug.Log(GameManager.i.State);
        if (GameManager.i.State == StateMachineStep.Free)
            this.gameObject.SetActive(false);*/
        StateChanged(GameManager.i.State);

        GameManager.i.DialogueEvents.OnDisplayDialogue += DisplayText;

        GameManager.i.DialogueEvents.OnDialogueFinished += CloseInspectMenu;

        GameManager.i.OnChangeState.AddListener(StateChanged);
    }

    public void DisplayText(string dialogueLine)
    {
        Debug.Log(dialogueLine);
        _text.text = dialogueLine;
    }

    private void ResetPanel()
    {
        _text.text = "";
    }

    public void StateChanged(StateMachineStep newState)
    {
        if (newState == StateMachineStep.Free)
        {
            //this.gameObject.SetActive(false);
            ResetTriggers();
            _animator.SetTrigger("Hide");
        }
            
        
        if (newState == StateMachineStep.Inspect)
        {
          //  ResetPanel();
            //this.gameObject.SetActive(true);
            ResetTriggers();
            _animator.SetTrigger("Show");
        }
            
    }

    public void CloseInspectMenu()
    {
        GameManager.ChangeState(StateMachineStep.Free);
    }

    private void ResetTriggers()
    {
       _animator.ResetTrigger("Show");
       _animator.ResetTrigger("Hide"); 
    }
}
