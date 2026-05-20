using System;
using UnityEngine;
using UnityHFSM;
public class WaitState : EnemyStateBase
{
    public WaitState(bool needsExitTime, Enemy enemy, float exitTime) : base(needsExitTime, enemy, exitTime)
    {
        base.OnEnter();
        _agent.isStopped = true;
        //Mimic is sleeping
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("this " + _exitTime);
        timer.Reset();
        _enemy.CheckedPlace = false;
    }

   /* public override void OnLogic()
    {
        base.OnLogic();
        if (timer.Elapsed >= _exitTime)
        {
            fsm.StateCanExit();
        }
        Debug.Log(timer.Elapsed);
    }*/
}
