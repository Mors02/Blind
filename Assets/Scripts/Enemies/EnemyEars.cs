using UnityEngine;
using UnityEngine.Events;

public class EnemyEars : MonoBehaviour
{    

    public UnityEvent<Vector3> OnSoundHeard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnSoundHeard = new UnityEvent<Vector3>();
    }

    /// <summary>
    /// Called when an enemy that can listen is in listening distance of the action
    /// </summary>
    /// <param name="position">Position of the origin of the sound</param>    
    public void Heard(Vector3 position)
    {
        OnSoundHeard?.Invoke(position);
    }

}
