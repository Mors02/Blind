using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Inventory/Item")]
public class Item : ScriptableObject
{
    public string ObjectId;
    public string ObjectName;
    public int Id;
    public GameObject PrefabWhenDropped;

    public string InkVariable;

}
