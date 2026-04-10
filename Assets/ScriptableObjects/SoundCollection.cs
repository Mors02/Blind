using System;
using FMODUnity;
using UnityEngine;
using System.Linq;
using FMOD;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "Scriptable Objects/SoundCollection")]
public class SoundCollection : ScriptableObject
{
    public SoundInstance[] Sounds;

    public EventReference GetSound(string id)
    {
        return Array.Find(Sounds, soundEvent => soundEvent.id == id).sound;
    }
}

[Serializable]
public class SoundInstance
{
    public EventReference sound;
    public string id;
}
