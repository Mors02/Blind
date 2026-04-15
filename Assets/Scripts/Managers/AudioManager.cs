using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene");
        }
        Instance = this;
    }

    /// <summary>
    /// Play a sound from the player position
    /// </summary>
    /// <param name="sound">Sound that needs to be played</param>
    public void PlayOneShot(EventReference sound)
    {
        //play a sound on the player position
        RuntimeManager.PlayOneShot(sound, GameManager.i.Player.transform.position);
    }

    /// <summary>
    /// Play a sound from a defined position
    /// </summary>
    /// <param name="sound">Sounds that needs to be played</param>
    /// <param name="from">Where the sounds should play</param>
    public void PlayOneShot(EventReference sound, Vector3 from)
    {
        RuntimeManager.PlayOneShot(sound, from);
    }
}
