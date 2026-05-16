using System;
using UnityEngine;
using UnityHFSM;

public class SeekState : EnemyStateBase
{
    public SeekState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy)
    {
        
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        _agent.SetDestination(_enemy.LastPlayerPosition);
    }
}
