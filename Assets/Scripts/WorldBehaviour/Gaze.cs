using Unity.Cinemachine;
using UnityEngine;

public class Gaze : MonoBehaviour
{
    private Transform _camera;
    private Transform _parent;
    [SerializeField]
    private NoiseSettings _scaredNoise;
    [SerializeField]
    private NoiseSettings _baseNoise;
    

    [SerializeField]
    private CinemachineBasicMultiChannelPerlin _cinemachine;
    void Start()
    {
        _camera = Camera.main.transform;
        _parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position-_camera.position); //look at the camera
        transform.position = _parent.position;
    }

    void OnBecameVisible()
    {
        Debug.Log("Rendered");
        _cinemachine.NoiseProfile = _scaredNoise;
    }

    void OnBecameInvisible()
    {
        Debug.Log("Not rendered");
        _cinemachine.NoiseProfile = _baseNoise;
    }


}
