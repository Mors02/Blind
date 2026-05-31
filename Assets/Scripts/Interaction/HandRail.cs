using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HandRail : MonoBehaviour
{
    [SerializeField]
    Collider _collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
    }
}
