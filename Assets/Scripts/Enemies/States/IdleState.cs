using System;
using UnityEngine;
using UnityHFSM;
public class IdleState : EnemyStateBase
{
    public IdleState(bool needsExitTime, Enemy enemy, float exitTime = 0.1F, Action<State<EnemyState, StateEvent>> onEnter = null, Action<State<EnemyState, StateEvent>> onLogic = null, Action<State<EnemyState, StateEvent>> onExit = null, Func<State<EnemyState, StateEvent>, bool> canExit = null) : base(needsExitTime, enemy, exitTime, onEnter, onLogic, onExit, canExit)
    {
    }
}
