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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if(_player == null)
        {
            Debug.LogWarning("No player found in the scene.");
        }
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.Log(_renderer.isVisible);
        if (_renderer.isVisible)
        {
            _navmeshAgent.enabled = false;
            //get nearest hiding spot
            HidingSpot nearest = GetNearestHidingSpot();
            //teleport to it
           this.transform.position = nearest.transform.position;

            //retrieve prefab form

            //instantiate it
            
        } else
        {
            _navmeshAgent.enabled = true;
            _navmeshAgent.SetDestination(_player.transform.position);
            //return to base form (remove any prefab instantiate under it)
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
