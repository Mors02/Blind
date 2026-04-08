using System;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
public class DialogueEvents
{
    public event Action<string> OnEnterDialogue;
    /// <summary>
    /// Called when enters the dialogue
    /// </summary>
    /// <param name="knotName">name of the starting ink knot for the dialogue</param>
    public void EnterDialogue(string knotName)
    {
        OnEnterDialogue?.Invoke(knotName);
    }

    /// <summary>
    /// Called when the dialogue actually starts
    /// </summary>
    public event Action OnDialogueStarted;
    public void DialogueStarted()
    {
        OnDialogueStarted?.Invoke();    
    }

    /// <summary>
    /// Called when the dialogue finishes (ink reached -> DONE or -> END)
    /// </summary>
    public event Action OnDialogueFinished;

    public void DialogueFinished()
    {
        OnDialogueFinished?.Invoke();
    }

    /// <summary>
    /// Called when the next string of the dialogue should be displayed
    /// </summary>
    public event Action<string, List<Choice>> OnDisplayDialogue;
    public void DisplayDialogue(string dialogueLine, List<Choice> choices)
    {
        OnDisplayDialogue?.Invoke(dialogueLine, choices);
    }
    
    public event Action OnDialoguePanelClose;
    public void CloseDialoguePanel()
    {
        OnDialoguePanelClose?.Invoke();
    }

    public event Action<int> OnChoiceSelected;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        OnChoiceSelected?.Invoke(choiceIndex);
    }

    public event Action<string, Ink.Runtime.Object> OnUpdateInkDialogueVariable;
    public void onUpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        OnUpdateInkDialogueVariable?.Invoke(name, value);
    }
}
