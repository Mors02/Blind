using UnityEngine;
using Ink.Runtime;
public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("PlaySound", (string soundId) => PlaySound(soundId));
        story.BindExternalFunction("CompleteAction", (string objectId, string actionId) => CompleteAction(objectId, actionId));
        story.BindExternalFunction("UpdateInventory", (string itemId, bool added) => UpdateInventory(itemId, added));    
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("PlaySound");
        story.UnbindExternalFunction("CompleteAction");
        story.UnbindExternalFunction("UpdateInventory");  
    }

    private void PlaySound(string soundId)
    {
        AudioManager.Instance.PlayOneShot(GameAssets.i.InteractionSounds.GetSound(soundId));
    }

    /// <summary>
    /// Add or remove an item from the inventory
    /// </summary>
    /// <param name="itemId">the item that should be added or removed</param>
    /// <param name="added">if true, it's addded otherwise it's removed</param>
    private void UpdateInventory(string itemId, bool added)
    {
        GameManager.UpdateInventory(itemId, added);
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
