using UnityEngine;

public enum EnemyState
{
    Idle,
    Seek,
    Alarm,
    Patrol,
    Attack,
    Wait
}

public enum StateEvent
{
    DetectPlayer,
    LostPlayer,
    SoundHeard
}
