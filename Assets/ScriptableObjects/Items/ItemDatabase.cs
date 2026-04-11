using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public Item[] AllItems;

    public Item GetItem(string id)
    {
        return Array.Find(AllItems, item => item.ObjectId == id);
    }
}
