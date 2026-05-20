using UnityHFSM;
using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class EnemyStateBase : State<EnemyState, StateEvent>
{
    protected readonly Enemy _enemy;
    protected readonly NavMeshAgent _agent;
    protected bool _requestedExit;
    protected float _exitTime;

    protected readonly Action<State<EnemyState, StateEvent>> _onEnter;
    protected readonly Action<State<EnemyState, StateEvent>> _onLogic;
    protected readonly Action<State<EnemyState, StateEvent>> _onExit;
    protected readonly Func<State<EnemyState, StateEvent>, bool> _canExit;

    public EnemyStateBase(bool needsExitTime, 
        Enemy enemy,
        float exitTime = 0.1f,
        Action<State<EnemyState, StateEvent>> onEnter = null,
        Action<State<EnemyState, StateEvent>> onLogic = null,
        Action<State<EnemyState, StateEvent>> onExit = null,
        Func<State<EnemyState, StateEvent>, bool> canExit = null
    )
    {
        this._enemy = enemy;
        this._onEnter = onEnter;
        this._onLogic = onLogic;
        this._onExit = onExit;
        this._canExit = canExit;
        this._exitTime = exitTime;
        this.needsExitTime = needsExitTime;
        _agent = _enemy.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _requestedExit = false;
        _onEnter?.Invoke(this);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (_requestedExit && timer.Elapsed >= _exitTime)
        {
            Debug.Log("State can exit from time");
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if (!this.needsExitTime || _canExit != null && _canExit(this))
        {
            fsm.StateCanExit();
        } else
        {
            _requestedExit = true;
            
        }
    }

}
