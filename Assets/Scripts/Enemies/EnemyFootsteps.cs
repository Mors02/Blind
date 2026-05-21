using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyFootsteps : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    [SerializeField]
    private string _footstepSoundName;

    /// <summary>
    /// How many steps in 10 seconds
    /// </summary>
    [Tooltip("Average step count every 10 seconds")] 
    [SerializeField]
    private float _baseStepRate;

    /// <summary>
    /// The base speed of the enemy (from the navMeshAgent speed)
    /// </summary>
    private float _baseSpeed;
    /// <summary>
    /// How many seconds between a step and another
    /// </summary>
    private float _stepTime;

    /// <summary>
    /// How many seconds for next step (accounting for randomness)
    /// </summary>
    private float _nextStepIn;

    private float _timer;

    /// <summary>
    /// Number between 0 and 1 that defines how much variation there can be in the step time (+- this value on the actual time between steps)
    /// </summary>
    [Tooltip("Number between 0 and 1 that defines how much variation there can be in the step time (+- this value on the actual time between steps)")]
    [SerializeField]
    private float _randomVariation = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._baseSpeed = _navMeshAgent.speed;
        _stepTime = 10 / _baseStepRate;
        _timer = 0;
        _nextStepIn = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_navMeshAgent.speed);
        Debug.Log(_navMeshAgent.velocity.magnitude);
        if (_navMeshAgent.velocity.magnitude > 0)
        {
            _timer += Time.deltaTime;
            //if I just started moving it should still be 0 so it enters in the check
            if (_timer >= _nextStepIn)
            {
                //play the sound
                AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound(_footstepSoundName), this.transform.position);

                //retrieve the new steptime
                //recover the actual step time based on the speed relative to the base speed
                float actualStepTime = _stepTime * _navMeshAgent.velocity.magnitude / _baseSpeed;

                float upperBound = actualStepTime + (actualStepTime * _randomVariation);
                float lowerBound = actualStepTime - (actualStepTime * _randomVariation);

                _nextStepIn = Random.Range(lowerBound, upperBound);
                _timer = 0;
            }

        }
        else
        {
            _nextStepIn = 0;
        }    
    }
}
