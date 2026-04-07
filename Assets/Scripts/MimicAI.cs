using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MimicAI : MonoBehaviour
{

    private GameObject _player;
    [SerializeField]
    private NavMeshAgent _navmeshAgent;

    [SerializeField]
    private Transform _hidingSpotParent;

    private Renderer _renderer;

    [SerializeField]
    private StudioEventEmitter _eventEmitter;

    [SerializeField]
    private float _minStepTime = 0.25f, _maxStepTime = 0.4f;
    private float _timePassed = 0, _randDuration = 0;

    private bool _isMoving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.LogWarning("No player found in the scene.");
        }
        _renderer = GetComponent<Renderer>();

        _randDuration = Random.Range(_minStepTime, _maxStepTime);
    }

    // Update is called once per frame
    void Update()
    {   
        //random time between steps
        if (_isMoving && _timePassed >= _randDuration)
        {
            _timePassed = 0;
            _randDuration = Random.Range(_minStepTime, _maxStepTime);
            _eventEmitter.Play();
        }

        _timePassed += Time.deltaTime;
        
        //check if the mimic is visible and should move
        if (_renderer.isVisible)
        {
            _navmeshAgent.enabled = false;
            //get nearest hiding spot
            HidingSpot nearest = GetNearestHidingSpot();
            //teleport to it
           this.transform.position = nearest.transform.position;
           
           //create the object to transform to (only if it was moving and now not anymore)
           if (_isMoving == true)           
            Instantiate(nearest.GetObject(), this.transform);
           
           _isMoving = false;
           _timePassed = 0;

            //retrieve prefab form

            //instantiate it
            
        } else
        {
            //if it was not moving then destroy (it should happen only the first time)
            if (!_isMoving)
            {
                //return to base form (remove any prefab instantiate under it)
                foreach (Transform child in this.transform.GetComponentsInChildren<Transform>())
                {
                    if (child.gameObject.GetEntityId() != this.gameObject.GetEntityId())
                        Destroy(child.gameObject);
                }    
            }

            _navmeshAgent.enabled = true;
            _navmeshAgent.SetDestination(_player.transform.position);
            _isMoving = true;


            
        }
        // _navmeshAgent.SetDestination(_player.transform.position);
    }

    public void TestHidingSpots()
    {
        HidingSpot nearest = GetNearestHidingSpot();
        //teleport to it
        this.transform.position = nearest.transform.position;
        Debug.Log("Teleported to " + nearest.name);
    }

    private HidingSpot GetNearestHidingSpot()
    {
        float nearestDistance = float.MaxValue;
        
        HidingSpot nearest = null;

        foreach(HidingSpot spot in _hidingSpotParent.GetComponentsInChildren<HidingSpot>())
        {
            Debug.Log("Distance from " + spot.name + " = " + Vector3.Distance(spot.transform.position, this.transform.position));
            float distance = Vector3.Distance(spot.transform.position, this.transform.position);
            if (distance < nearestDistance)
            {
                Debug.Log("New nearest " + spot.name);
                nearestDistance = distance;
                nearest = spot;
            }
        }

        return nearest;
    }
}
