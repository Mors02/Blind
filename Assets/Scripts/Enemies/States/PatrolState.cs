using System.Collections;
using TMPro;
using UnityEngine;
using System;
using UnityHFSM;

public class PatrolState : EnemyStateBase
{
    private Transform[] _checkpoints;
    private int _currentPoint = 0;
    private float _timer, _timeToWait, _soundTimer, _soundTimeToWait;
    private float _minWaitTime = 1f, _maxWaitTime = 4f;
    private float _speed;
    public PatrolState(bool needsExitTime, Enemy enemy, Action<State<EnemyState, StateEvent>> onEnter, float speed) : base(needsExitTime, enemy, onEnter: onEnter)
    {
        this._checkpoints = enemy.PatrolPoints;
        _currentPoint = 0;
        _timer = _timeToWait = _soundTimer = _soundTimeToWait = 0;
        this._speed = speed;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this._agent.speed = _speed;

        //if the patrol path is not configured correctly, exit the state
        if (_checkpoints.Length <= 0)
            fsm.StateCanExit();

        _agent.enabled = true;
        _agent.isStopped = false;
        _agent.SetDestination(_checkpoints[_currentPoint].position);
        _timeToWait = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
        _soundTimeToWait = UnityEngine.Random.Range(_minWaitTime*2, _maxWaitTime*2);
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

         if (_soundTimer >= _soundTimeToWait)
        {
            _soundTimeToWait = UnityEngine.Random.Range(_minWaitTime*2, _maxWaitTime*2);
            AudioManager.Instance.PlayOneShot(GameAssets.i.EnemySounds.GetSound("SeekerInPatrol"), _enemy.transform.position);
            _soundTimer = 0;
        }
        _soundTimer = _soundTimer + Time.deltaTime;
        
    }
}
