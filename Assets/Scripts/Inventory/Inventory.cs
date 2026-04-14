using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Inventory : MonoBehaviour
{
    
    [SerializeField]
    private List<Item> _items;

    public List<Item> Items => _items;
    /// <summary>
    /// Called when a new Item is added to the inventory
    /// </summary>
    public Action<Item> OnItemAdded;

    /// <summary>
    /// Called when an Item is removed from the inventory
    /// </summary>
    public Action<Item> OnItemRemoved;
    /// <summary>
    /// Called whenever the inventory changes (both add and remove)
    /// </summary>
    public Action OnInventoryChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _items = new List<Item>();
        //OnInventoryChanged.Invoke();
    }

    /// <summary>
    /// Add an item to the inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(Item item)
    {        
        this._items.Add(item);
        OnItemAdded?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    /// <param name="itemId">Id of the item to remove</param>
    public void RemoveFromInventory(string itemId)
    {
        Item item = this._items.Find(item => item.ObjectId == itemId);

        this._items.Remove(item);

        OnItemRemoved?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Remove item from inventory
    /// </summary>
    /// <param name="item">item to remove</param>
    public void RemoveFromInventory(Item item)
    {
        this._items.Remove(item);

        OnItemRemoved?.Invoke(item);
        OnInventoryChanged?.Invoke();
    }
}
