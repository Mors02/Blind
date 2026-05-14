using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour {
    
    //private Animator _animator;
    private NavMeshAgent _agent;
    private StateMachine<EnemyState, StateEvent> _enemyFSM;

    private void Awake()
    {
        //_animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        _enemyFSM.AddState(EnemyState.Seek, new SeekState(false, this));
        _enemyFSM.AddState(EnemyState.Patrol, new PatrolState(false, this));
        _enemyFSM.AddState(EnemyState.Alarm, new AlarmState(false, this));

        _enemyFSM.SetStartState(EnemyState.Idle);

        _enemyFSM.Init();
    }

    private void Update()
    {
        _enemyFSM.OnLogic();
    }

}