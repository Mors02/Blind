using UnityEngine;
using Unity.Cinemachine;

public class ReflectRotation : MonoBehaviour
{   
    [SerializeField]
    private Transform _cameraPos;    
    // Update is called once per frame
    void Update()
    {
         float yRotation = _cameraPos.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
