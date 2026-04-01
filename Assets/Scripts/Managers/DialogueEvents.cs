using System;
using UnityEngine;

public class DialogueEvents
{
    public event Action<string> OnEnterDialogue;

    public void EnterDialogue(string knotName)
    {
        OnEnterDialogue?.Invoke(knotName);
    }
}
