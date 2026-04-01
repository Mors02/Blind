using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        /*Debug.Log(GameManager.i.State);
        if (GameManager.i.State == StateMachineStep.Free)
            this.gameObject.SetActive(false);*/
        StateChanged(GameManager.i.State);

        GameManager.i.OnChangeState.AddListener(StateChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
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
