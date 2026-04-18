using UnityEngine;

public class Elevator : MonoBehaviour
{


    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.transform.name);
        other.transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider other)
    {
         Debug.Log(other.transform.name);
        other.transform.SetParent(transform); 
    }

}
