using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy : MonoBehaviour
{

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
    [SerializeField]
    private string _activeState;

    [Header("State infos")]
    public Vector3 LastPlayerPosition;
    public bool CheckedPlace = false;
    public Transform[] PatrolPoints;
    [SerializeField]
    private float _waitStateTime;
    [SerializeField]
    private float _baseSpeed, _alarmSpeed;

    private void Awake()
    {

        _player = GameObject.FindGameObjectWithTag("Player");
        //_animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<EnemyState, StateEvent>();

        //add states to the state machine
        _enemyFSM.AddState(EnemyState.Idle, new IdleState(false, this));
        _enemyFSM.AddState(EnemyState.Seek, new SeekState(false, this, onEnter: PlaySoundOnEnter, speed: _baseSpeed));
        _enemyFSM.AddState(EnemyState.Wait, new WaitState(true, this, _waitStateTime, onEnter: PlaySoundOnEnter));
        _enemyFSM.AddState(EnemyState.Patrol, new PatrolState(false, this, onEnter: PlaySoundOnEnter, speed: _baseSpeed));
        _enemyFSM.AddState(EnemyState.Alarm, new AlarmState(false, this, _player.transform, onEnter: PlaySoundOnEnter, speed: _alarmSpeed));

        //add transitions that are triggered from events
        _enemyFSM.AddTriggerTransition(StateEvent.SoundHeard, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Seek));
        _enemyFSM.AddTriggerTransition(StateEvent.SoundHeard, new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Seek));
        _enemyFSM.AddTriggerTransition(StateEvent.SoundHeard, new Transition<EnemyState>(EnemyState.Wait, EnemyState.Seek, forceInstantly: true));

        _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Seek, EnemyState.Alarm, forceInstantly: true));
        _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Wait, EnemyState.Alarm, forceInstantly: true));

        _enemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Alarm, EnemyState.Seek));

        //add transitions that are checked every call on _enemyFSM.OnLogic()       
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Seek, EnemyState.Wait, (transition) => CheckedPlace));
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Wait, EnemyState.Patrol));
        //Failsafe transiton if the patrol path is not configured
        _enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Idle, (transition) => PatrolPoints.Length <= 0));

        // _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Idle, EnemyState.Alarm));
        // _enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer, new Transition<EnemyState>(EnemyState.Patrol, EnemyState.Alarm));
        // _enemyFSM.AddTriggerTransition(StateEvent.LostPlayer, new Transition<EnemyState>(EnemyState.Alarm, EnemyState.Patrol));





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
        this._activeState = _enemyFSM.ActiveStateName.ToString();
    }

    private void PlaySoundOnEnter(State<EnemyState, StateEvent> State)
    {
        switch (State.name)
        {
            case EnemyState.Seek:
                AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound("SeekerEnterSeek"), transform.position);
                break;

            case EnemyState.Wait:
                AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound("SeekerEnterWait"), transform.position);
                break;

            case EnemyState.Alarm:
                AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound("SeekerEnterAlert"), transform.position);
                break;
                
            case EnemyState.Patrol:
                AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound("SeekerInPatrol"), transform.position);
                break;

        }
    }
}