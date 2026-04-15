using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IActionHandler
{
    //[SerializeField]
    //private CanvasManager _canvasManager;
    [SerializeField]
    private GameObject _menuSection;
    [SerializeField]
    private GameObject _itemsSection;
    [SerializeField]
    private Transform _itemsContainer;

    [SerializeField]
    private TextFadeTransition _textFade;

    [SerializeField]
    private string _inputText;
    public void Execute(string actionId)
    {
        ShowInventory();
    }

    /// <summary>
    /// Show the inventory
    /// </summary>
    public void ShowInventory(Item item = null)
    {
        StartCoroutine("FadeInInventory");
        _itemsSection.SetActive(true);
    }
    /// <summary>
    /// Default state when opening, the menu is open and the inventory is closed
    /// </summary>
    public void ShowInventoryMenu(bool firstTime)
    {   
        //play the clip only the first time you open the inventory (not if you're closing the items section)
        if (firstTime)
            AudioManager.Instance.PlayOneShot(GameAssets.i.UISounds.GetSound("UIOpen"));
        _menuSection.SetActive(true);
        _itemsSection.SetActive(false);
        _textFade.FadeIn(_inputText);
    }

    /// <summary>
    /// Called when visualizing the inventory
    /// </summary>
    public void ShowItems()
    {
        _menuSection.SetActive(false);
        _textFade.FadeOut();
        ShowInventory();
    }

    /// <summary>
    /// Starts writing the objective
    /// </summary>
    public void ShowObjective()
    {
        _textFade.TransitionTo("I need to get out of here.");
    }

    /// <summary>
    /// Visually adds an item one at a time.
    /// </summary>
    /// <returns>Coroutine</returns>
    public IEnumerator FadeInInventory()
    {
        //wait for the next frame so that it doesnt conflict with the closing of the dialogue
        //yield return new WaitForNextFrameUnit();
        //go back to inventory
        //GameManager.ChangeState(StateMachineStep.Inventory);
        //add all the items to the container
        foreach (Transform child in _itemsContainer.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject != _itemsContainer.gameObject)
                Destroy(child.gameObject);
        }

        yield return new WaitForEndOfFrame();

        foreach (Item item in GameManager.i.Inventory.Items)
        {
            GameObject prefab = Instantiate(GameAssets.i.InventoryItemPrefab, _itemsContainer);
            prefab.GetComponent<ItemUI>().Setup(item);
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Hide the inventory
    /// </summary>
    public void HideInventory()
    {
        //set the inventory section hidden so that next time it starts from a blank slate
        _itemsSection.SetActive(false);
        _menuSection.SetActive(false);
        _textFade.FadeOut();
       
    }

    /// <summary>
    /// Hides the inventory and goes back to free mode
    /// </summary>
    public void CloseInventory()
    {
        HideInventory();
        //makes the inventory close
        GameManager.ChangeState(StateMachineStep.Free);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this._canvasManager = gameObject.GetComponent<CanvasManager>();

        //GameManager.RegisterObject("inventory", this);
        GameManager.i.Inventory.OnItemRemoved += ShowInventory;
        GameManager.i.OnChangeState.AddListener(ChangeState);
        _menuSection.SetActive(false);
        _itemsSection.SetActive(false);

    }

    private void ChangeState(StateMachineStep newState, StateMachineStep oldState)
    {
        switch (newState)
        {
            case StateMachineStep.Free:
                if (oldState == StateMachineStep.Inspect)
                    HideInventory();
                break;
            case StateMachineStep.Inspect:
                HideInventory();
                break;
            case StateMachineStep.Inventory:
                ShowInventoryMenu(true);
                break;
        }
    }

    void OnDestroy()
    {
        GameManager.UnregisterObject("inventory");
    }

}
