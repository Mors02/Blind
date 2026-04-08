using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using Unity.VisualScripting;
public class InkDialogueVariables
{   
    /// <summary>
    /// List of variables present inside the ink file
    /// </summary>
    private Dictionary<string, Ink.Runtime.Object> _variables;

    public InkDialogueVariables(Story story)
    {
        _variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in story.variablesState)
        {
            Ink.Runtime.Object value = story.variablesState.GetVariableWithName(name);
            _variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    /// <summary>
    /// Sync all variables and then subscribes to every subsequent event
    /// </summary>
    /// <param name="story">the current ink story</param>
    public void SyncVariablesAndStartListening(Story story)
    {
        SyncVariablesToStory(story);
        //if a variable changes in the ink file, our C# dictionary will stay up to date
        story.variablesState.variableChangedEvent += UpdateVariableState;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="story"></param>
    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= UpdateVariableState;
    }

    /// <summary>
    /// Updates the value of the dictionary 
    /// </summary>
    /// <param name="name">name of the variable that should be set</param>
    /// <param name="value">value</param>
    public void UpdateVariableState(string name, Ink.Runtime.Object value)
    {
        if (!_variables.ContainsKey(name))
        {
            Debug.LogWarning("Variable named " + name + " not found in dictionary.");
            return;
        }
    
        _variables[name] = value;
        Debug.Log("Updated dialogue variable: " + name + " = " + value);
    }

    /// <summary>
    /// Helper function used to sync all the variables at the start
    /// </summary>
    /// <param name="story">the current ink story</param>
    private void SyncVariablesToStory(Story story)
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
