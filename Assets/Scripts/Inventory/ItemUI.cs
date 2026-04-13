using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private Button _button;
    private string _itemId;

    /// <summary>
    /// Setup function that should be called on the creation of the object
    /// </summary>
    /// <param name="name">name of item shown on the UI</param>
    /// <param name="id">id of the item that should be removed on click</param>
    public void Setup(string name, string id)
    {
        this._text.text = name;
        this._itemId = id;
        this._button.onClick.AddListener(delegate { RemoveItem(_itemId); });
    }
    
    /// <summary>
    /// Setup function that should be called on the creation of the object
    /// </summary>
    /// <param name="item">Item in the inventory</param>
    public void Setup(Item item)
    {
        this._text.text = item.ObjectName;
        this._itemId = item.ObjectId;
        this._button.onClick.AddListener(delegate { RemoveItem(_itemId); });
    }

    /// <summary>
    /// Remove the selected item from the inventory
    /// </summary>
    /// <param name="id">item that should be removed</param>
    public void RemoveItem(string id)
    {
        //GameManager.i.Inventory.RemoveFromInventory(id);
        Debug.Log("Removed item: " + id);
    }
}
