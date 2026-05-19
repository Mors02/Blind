using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityHFSM;

public class SeekState : EnemyStateBase
{
    float _timer, _timeToWait = 5f;
    Vector3 _targetPosition;
    public SeekState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy)
    {
        
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        _enemy.CheckedPlace = false;
        _agent.enabled = true;
        _agent.isStopped = false;
        //this is set from the events inside Enemy.cs
        _targetPosition = _enemy.LastPlayerPosition;
        _agent.SetDestination(_targetPosition);
        _timer = 0;
    }

    public override void OnLogic()
    {
        base.OnLogic();
        
        
        if (Vector3.Distance(this._enemy.transform.position, _targetPosition) <= 2f)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToWait)
            {
                _enemy.CheckedPlace = true;
                Debug.Log("Can exit");
                fsm.StateCanExit();     
            }
           
        }
    }
}
