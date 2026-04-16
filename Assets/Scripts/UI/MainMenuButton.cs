using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Animator _animator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ResetTriggers();
        _animator.SetTrigger("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetTriggers();
        _animator.SetTrigger("Exit");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!gameObject.TryGetComponent(out _animator))
        {
            Debug.LogWarning("No animator on " + name);
        }
    }

    private void ResetTriggers()
    {
        _animator.ResetTrigger("Enter");
        _animator.ResetTrigger("Exit");
    }
}
