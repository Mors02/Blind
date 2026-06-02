using System;
using FMODUnity;
using TMPro;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Text))]
public class DynamicWorldText : MonoBehaviour
{

    private Transform _camera;
    private Transform _parent; 

    private TMP_Text _text;   

    [SerializeField]
    private DynamicText _dynamicText;

    [SerializeField]
    private StudioEventEmitter _eventEmitter;

    [SerializeField]
    private Image _earTop;
    [SerializeField]
    private Image _earBottom;

    [SerializeField]
    [Range(1, 100)]
    private float _range;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main.transform;
        _parent = transform.parent;
        _text = gameObject.GetComponent<TMP_Text>();
        _dynamicText.Start();
        if (!_eventEmitter.enabled)
        {
            _earTop.enabled = false;
            _earBottom.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-_camera.position); //look at the camera
        transform.position = _parent.position;
        _text.text = _dynamicText.GetText(Time.deltaTime);

        //get the distance to the camera
        float distance = Vector3.Distance(transform.position, _camera.position);
        
        //the alpha should be low if distant and high if near
        float alpha = Mathf.Clamp(_range / distance, 0f, 0.3f);
        Debug.Log("Alpha: " + alpha);

        ChangeAlpha(alpha);
    }

    /// <summary>
    /// Change alpha based on distance. Pass the alpha value calculated.
    /// </summary>
    /// <param name="alpha">alpha of the current images</param>
    private void ChangeAlpha(float alpha)
    {   //white color with corresponding alpha
        _earTop.color = new Color(1f, 1f, 1f, alpha);
        _earBottom.color = new Color(1f, 1f, 1f, alpha);
        _text.color =  new Color(1f, 1f, 1f, alpha);
    }


}



