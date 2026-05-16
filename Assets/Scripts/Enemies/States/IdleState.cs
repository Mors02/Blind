using System;
using UnityEngine;
using UnityHFSM;
public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy)
    {
        base.OnEnter();
        _agent.isStopped = true;
        //Mimic is sleeping
    }
}
