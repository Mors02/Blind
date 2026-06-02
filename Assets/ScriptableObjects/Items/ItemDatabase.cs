using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public Item[] AllItems;

    public Item GetItem(string id)
    {
        Debug.Log("id: " + id);
        return Array.Find(AllItems, item => {Debug.Log(item.ObjectId); return item.ObjectId == id;});
    }
}
