using UnityEngine;

public class Door : MonoBehaviour, IActionHandler
{
    public void Execute(string actionId)
    {
        if (actionId == "open")
            Debug.Log("Pierino has opened the door");
        else
            Debug.Log("Pierino has closed the door");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.RegisterObject("door", this);
    }

    void OnDestroy()
    {
        GameManager.UnregisterObject("door");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
