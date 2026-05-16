using System;
using UnityEngine;
using UnityHFSM;

public class AlarmState : EnemyStateBase
{
    private Transform _target;

    public AlarmState(bool needsExitTime, Enemy enemy, Transform target) : base(needsExitTime, enemy)
    {
        this._target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;

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
