using UnityEngine;

public enum EnemyState
{
    Idle,
    Seek,
    Alarm,
    Patrol,
    Attack
}

public enum StateEvent
{
    DetectPlayer,
    LostPlayer
}
