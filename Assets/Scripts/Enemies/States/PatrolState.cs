using System.Collections;
using TMPro;
using UnityEngine;
using UnityHFSM;

public class PatrolState : EnemyStateBase
{
    private Transform[] _checkpoints;
    private int _currentPoint = 0;
    private float _timer, _timeToWait;
    private float _minWaitTime = 1f, _maxWaitTime = 4f;
    public PatrolState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy)
    {
        this._checkpoints = enemy.PatrolPoints;
        _currentPoint = 0;
        _timer = 0; _timeToWait = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Entered Patrol");

        //if the patrol path is not configured correctly, exit the state
        if (_checkpoints.Length <= 0)
            fsm.StateCanExit();

        _agent.enabled = true;
        _agent.isStopped = false;
        Debug.Log("Next destination " + _checkpoints[_currentPoint].name);
        _agent.SetDestination(_checkpoints[_currentPoint].position);
        _timeToWait = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        //if we reach the position, check next position
        if (Vector3.Distance(_checkpoints[_currentPoint].position, _enemy.transform.position) <= 1f)
        {
            if (_timer >= _timeToWait)
            {
                _timeToWait = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
                _currentPoint = (_currentPoint + 1) % _checkpoints.Length;
                _agent.SetDestination(_checkpoints[_currentPoint].position);
                _timer = 0;
            }
            _timer = _timer + Time.deltaTime;
        }
    }
}
