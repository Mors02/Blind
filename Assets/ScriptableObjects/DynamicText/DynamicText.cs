using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DynamicText", menuName = "Scriptable Objects/DynamicText")]
public class DynamicText : ScriptableObject
{
    [SerializeField]
    private TextState[] _states;

    private float _passedTime = 0;
    private int _currentState = 0;

    public float TotalLoopDuration
    { 
        get {
            float sum = 0;
            Array.ForEach(_states, delegate(TextState state) { sum += state.Duration; }); 
            return sum;
        }
    }

    public void Start()
    {
        this._currentState = 0;
        this._passedTime = 0;
    }
    /// <summary>
    /// Function used to retrieve the current state of the text based on current time
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    /// <returns></returns>
    public string GetText(float deltaTime)
    {
        //if the duration is 0 then the text is static
        if (_states[_currentState].Duration <= 0)
            return _states[_currentState].Text;

        _passedTime += deltaTime;
        //if enough time has passed in the state
        if (_passedTime >= _states[_currentState].Duration)
        {
            //change state and go to the next, or loop if reached the end
            _currentState = ++_currentState % _states.Length;
            _passedTime = 0;
        }

        //return the current state text;
        return _states[_currentState].Text;
    }
}

/// <summary>
/// Represents one state the dynamic text can appear in
/// </summary>
[Serializable]
public class TextState
{
    public string Text;  
    public float Duration;      

    public TextState(float duration, string text)
    {
        this.Duration = duration;
        this.Text = text;    
    }
}