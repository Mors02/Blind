using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DynamicWorldText : MonoBehaviour
{

    private Transform _camera;
    private Transform _parent; 

    private TMP_Text _text;   

    [SerializeField]
    private DynamicText _dynamicText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main.transform;
        _parent = transform.parent;
        _text = gameObject.GetComponent<TMP_Text>();
        _dynamicText.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-_camera.position); //look at the camera
        transform.position = _parent.position;
        _text.text = _dynamicText.GetText(Time.deltaTime);
    }
}



