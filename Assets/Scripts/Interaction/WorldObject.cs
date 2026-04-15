using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [SerializeField]
    private WorldObjectInfo _info;

    public void PlayTouchSound(PrintType printType)
    {
        if (!_info.SoundOnTouch.IsNull)
            AudioManager.Instance.PlayOneShot(_info.SoundOnTouch, transform.position);
    }

    /// <summary>
    /// the text that appears when touching the object
    /// </summary>
    /// <returns></returns>
    public string Text
    {
        get {return this._info.ObjectName;}
    }

    /// <summary>
    /// the name of the knot in the ink script to retrieve the interactions
    /// </summary>
    /// <returns></returns>
    public string InkKnot
    {
        get {return this._info.knotId;}
    }
}
