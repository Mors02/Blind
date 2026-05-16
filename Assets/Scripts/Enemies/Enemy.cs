using System;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator)), RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour {
    
    private GameObject _player;
    private NavMeshAgent _agent;
    private StateMachine<EnemyState, StateEvent> _enemyFSM;

    [Header("Sensors")]
    [SerializeField]
    private EnemyEars _enemyEars;
    [SerializeField]
    private PlayerSensor _alarmRangeSensor;

    [Header("Debug")]
    [SerializeField]
    private bool _isInAlarmRange;

    [Header("State infos")]
    public Vector3 LastPlayerPosition;

    private void Awake()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        //_animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        //add states to the state machine
        _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        _enemyFSM.AddState(EnemyState.Seek, new SeekState(false, this));
        _enemyFSM.AddState(EnemyState.Patrol, new PatrolState(false, this));
        _enemyFSM.AddState(EnemyState.Alarm, new AlarmState(false, this, _player.transform));

        //add transitions that are triggered from events
        _enemyFSM.AddTriggerTransition(StateEvent.SoundHeard, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Seek));

       // _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Alarm));
       // _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Alarm));
       // _enemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Alarm, EnemyState.Patrol));

        //add transitions that are checked every call on _enemyFSM.OnLogic()
       // _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Seek, EnemyState.Patrol, (transition) => false));



        _enemyFSM.SetStartState(EnemyState.Idle);

        _enemyFSM.Init();
    }

    private void Start()
    {
        _alarmRangeSensor.OnPlayerEnter += AlarmRangeEnter;
        _alarmRangeSensor.OnPlayerExit += AlarmRangeExit;
        _enemyEars.OnSoundHeard.AddListener(SoundHeard);
    }

    private void AlarmRangeEnter(Transform player)
    {
        //Player is inside the alarm range, call the trigger on detect player
        _enemyFSM.Trigger(StateEvent.DetectPlayer);
        _isInAlarmRange = true;
    }

    private void AlarmRangeExit(Vector3 lastPosition)
    {
        LastPlayerPosition = lastPosition;
        _enemyFSM.Trigger(StateEvent.LostPlayer);
        _isInAlarmRange = false;  
    }

    private void SoundHeard(Vector3 position)
    {
        LastPlayerPosition = position;
        _enemyFSM.Trigger(StateEvent.SoundHeard);
    }

    private void Update()
    {
        _enemyFSM.OnLogic();
    }



    private void OnAttack(State<EnemyState, StateEvent> state)
    {
        
    }
    private void OnBounce(State<EnemyState, StateEvent> state)
    {
        
    }
    private void OnRoll(State<EnemyState, StateEvent> state)
    {
        
    }

}