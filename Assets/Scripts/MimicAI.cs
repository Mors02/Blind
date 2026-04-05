using UnityEngine;
using UnityEngine.AI;

public class MimicAI : MonoBehaviour
{

    private GameObject _player;
    [SerializeField]
    private NavMeshAgent _navmeshAgent;

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
        if (!_renderer.isVisible)
        {
            _navmeshAgent.enabled = true;
            _navmeshAgent.SetDestination(_player.transform.position);    
        } else
        {
            _navmeshAgent.enabled = false;
        }
        
    }
}
