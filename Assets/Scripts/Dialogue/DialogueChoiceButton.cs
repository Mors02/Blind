using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DialogueChoiceButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler//  IPointerClickHandler 
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private TMP_Text _text;

    private int _choiceIndex = -1;

    public void SetChoiceText(string choiceText)
    {
        _text.text = choiceText;
    }

    /// <summary>
    /// Set the choice index in the button to continue with the story based on the path taken
    /// </summary>
    /// <param name="choiceIndex"></param>
    public void SetChoiceIndex(int choiceIndex)
    {
        _choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        _button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.i.DialogueEvents.UpdateChoiceIndex(_choiceIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShot(GameAssets.i.UISounds.GetSound("MouseHover"));
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.i.DialogueEvents.UpdateChoiceIndex(_choiceIndex);
    }*/
}
