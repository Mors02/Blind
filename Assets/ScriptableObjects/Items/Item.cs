using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Inventory/Item")]
public class Item : ScriptableObject
{
    /// <summary>
    /// The string that will be inside ~ UpdateInventory({ObjectId}, true) inside the ink script
    /// </summary>
    [Tooltip("The string that will be inside ~ UpdateInventory({ObjectId}, true) inside the ink script")]
    public string ObjectId;
    /// <summary>
    /// The name that will be displayed in the inventory
    /// </summary>
    [Tooltip("The name that will be displayed in the inventory")]
    public string ObjectName;
    public int Id;

    /// <summary>
    /// The prefab that is instantiated when dropped on the floor
    /// </summary>
    [Tooltip("The prefab that is instantiated when dropped on the floor")]
    public GameObject PrefabWhenDropped;

    /// <summary>
    /// The name of the ink variable that contains POSSESSED or NOT_POSSESSED
    /// </summary>
    [Tooltip("The name of the ink variable that contains POSSESSED or NOT_POSSESSED")]
    public string InkVariable;

    public override string ToString()
    {
        return "(" + ObjectId + ") " + ObjectName;
    }
}
