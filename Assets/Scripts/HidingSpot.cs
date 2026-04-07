using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField]
    private GameObject _transformationObject;

    public GameObject GetObject()
    {
        return _transformationObject;
    }
}
