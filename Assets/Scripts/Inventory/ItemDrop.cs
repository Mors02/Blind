using UnityEngine;

[RequireComponent(typeof(WorldObject))]
public class ItemDrop : MonoBehaviour
{
    private WorldObject _object;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.i.Inventory.OnItemAdded += DestroyPrefab;
        if (!gameObject.TryGetComponent(out _object)) {
            Debug.LogWarning("Prefab without WorldObject script!");
        }
    }

    /// <summary>
    /// Cleanup called when the prefab is destroyed
    /// </summary>
    void OnDestroy()
    {
        GameManager.i.Inventory.OnItemAdded -= DestroyPrefab;        
    }

    private void DestroyPrefab(Item item)
    {
        //if the item i remove is the same of this prefab
        if (item.ObjectId == _object.InkKnot)
        //destroy the prefab
            Destroy(gameObject);
    }
}
