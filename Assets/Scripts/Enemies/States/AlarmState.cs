using System;
using UnityEngine;
using UnityHFSM;

public class AlarmState : EnemyStateBase
{
    private Transform _target;
    private float _speed;
    public AlarmState(bool needsExitTime, Enemy enemy, Transform target, Action<State<EnemyState, StateEvent>> onEnter, float speed) : base(needsExitTime, enemy, onEnter: onEnter)
    {
        this._target = target;
        this._speed = speed;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        this._agent.speed = _speed;

    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_requestedExit)
        {
            _agent.SetDestination(_target.position);
        } else if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            fsm.StateCanExit();
        }
    }
}
