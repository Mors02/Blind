using UnityEngine;
using Ink.Runtime;
public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("PlaySound", (string soundId) => PlaySound(soundId));
        story.BindExternalFunction("CompleteAction", (string objectId, string actionId) => CompleteAction(objectId, actionId));    
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("PlaySound");
        story.UnbindExternalFunction("CompleteAction");  
    }

    private void PlaySound(string soundId)
    {
        
    }

    private void CompleteAction(string objectId, string actionId = "")
    {
        GameManager.CompleteAction(objectId, actionId);

    }
}

/// <summary>
/// Interface used by all the objects that have functions that can be executed directly from ink
/// </summary>
public interface IActionHandler
{
    void Execute(string actionId);
}
