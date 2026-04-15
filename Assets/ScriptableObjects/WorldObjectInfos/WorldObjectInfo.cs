using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "WorldObjectInfo", menuName = "Scriptable Objects/WorldObjectInfo")]
public class WorldObjectInfo : ScriptableObject
{
    /// <summary>
    /// name of the object
    /// </summary>
    public string ObjectName;

    /// <summary>
    /// id of the knot for the dialogues of the object
    /// </summary>
    public string knotId;

    /// <summary>
    /// Sound played when touching with the object
    /// </summary>
    public EventReference SoundOnTouch;

}
