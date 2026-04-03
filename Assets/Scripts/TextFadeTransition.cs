using System.Collections;
using TMPro;
using UnityEngine;

public class TextFadeTransition : MonoBehaviour
{
    [SerializeField] private TMP_Text _textA;
    [SerializeField] private TMP_Text _textB;

    /// <summary>
    /// How long the total transition should be
    /// </summary>
    [SerializeField] private float _fadeDuration = 1.0f;
    /// <summary>
    /// The time between the fade out of the previous text and the fade in of the next text
    /// </summary>
    [SerializeField] [Range(0f, 1f)] private float _appearDelay = 0.3f;
    /// <summary>
    /// the size of the gradient (how many characters are partially visible during the transition)
    /// </summary>
    [SerializeField] [Range(0.05f, 0.5f)] private float _gradientWidth = 0.2f;

    private TMP_Text _current;
    private TMP_Text _next;

    private enum State { Idle, FadingIn, Transitioning, FadingOut }
    private State _state = State.Idle;

    private void Start()
    {
        _current = _textA;
        _next = _textB;

        //they start invisible
        SetAlpha(_current, 0f);
        SetAlpha(_next, 0f);
    }

    /// <summary>
    /// Fade in a text in the ui from left to right. To be used when it's the first text to be visualized
    /// </summary>
    /// <param name="newText">the string that should appear</param>
    public void FadeIn(string newText)
    {
        //in case of spamming, only one transition at a time
        StopAllCoroutines();
        FinalizeState();

        //changes text, starts fade in
        _current.text = newText;
        StartCoroutine(RunFadeIn());
    }

    /// <summary>
    /// Fades out the previous text and fades in the new text
    /// </summary>
    /// <param name="newText">the string that should appear</param>
    public void TransitionTo(string newText)
    {
        //in case of spamming, only one transition at a time
        StopAllCoroutines();
        FinalizeState();

        //changes text, starts transition
        _next.text = newText;
        StartCoroutine(RunTransition());
    }

    /// <summary>
    /// Fades out the last text of the dialogue
    /// </summary>
    public void FadeOut()
    {
        //in case of spamming, only one transition at a time
        StopAllCoroutines();
        FinalizeState();

        //changes text, starts fade out
        StartCoroutine(RunFadeOut());
    }

    /// <summary>
    /// Snaps the interrupted animation to its completed state before starting a new one,
    /// ensuring _current always holds the fully visible text going into the next operation.
    /// </summary>
    private void FinalizeState()
    {
        switch (_state)
        {
            case State.FadingIn:
                // _current was fading in — snap it to fully visible
                SetAlpha(_current, 1f);
                break;

            case State.Transitioning:
                // _current was fading out, _next was fading in — complete the swap
                SetAlpha(_current, 0f);
                SetAlpha(_next, 1f);
                (_current, _next) = (_next, _current);
                break;

            case State.FadingOut:
                // _current was fading out — snap it to fully invisible
                SetAlpha(_current, 0f);
                break;
        }

        _state = State.Idle;
    }

    /// <summary>
    /// Coroutine that handles the fade in of the first text
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunFadeIn()
    {
        //set the state
        _state = State.FadingIn;
        float elapsed = 0f;

        // forces the regeneration of the text object and applies the fade at the start
        _current.ForceMeshUpdate();
        ApplyFade(_current, 0f, false);

        //for the duration of the transition
        while (elapsed < _fadeDuration)
        {
            //increment time
            elapsed += Time.deltaTime;
            //fade out old text
            float t = Mathf.Clamp01(elapsed / _fadeDuration);
            ApplyFade(_current, t, false);
            yield return null;
        }

        //at the end make the whole text statically shown
        SetAlpha(_current, 1f);
        _state = State.Idle;
    }

    private IEnumerator RunTransition()
    {
        //set the state
        _state = State.Transitioning;
        float elapsed = 0f;

        // forces the regeneration of the text object and applies the fade at the start
        _current.ForceMeshUpdate();
        _next.ForceMeshUpdate();

        ApplyFade(_current, 0f, true);
        ApplyFade(_next, 0f, false);

        //while the duration is active
        while (elapsed < _fadeDuration)
        {
            //increment time
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fadeDuration);

            //fade out old text
            ApplyFade(_current, t, true);

            //after a delay
            float tIn = Mathf.Clamp01((t - _appearDelay) / (1f - _appearDelay));
            //fade in new text
            ApplyFade(_next, tIn, false);

            yield return null;
        }

        //at the end make the whole text statically hidden
        SetAlpha(_current, 0f);
        SetAlpha(_next, 1f);

        //invert the two text for a seamless transition between multiple texts in a row
        (_current, _next) = (_next, _current);
        _state = State.Idle;
    }

    /// <summary>
    /// Starts the fade out of the text
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunFadeOut()
    {   
        //set the state
        _state = State.FadingOut;
        float elapsed = 0f;

        // forces the regeneration of the text object and applies the fade at the start
        _current.ForceMeshUpdate();
        ApplyFade(_current, 0f, true);
        
        //while the duration is active
        while (elapsed < _fadeDuration)
        {
            //increment time
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fadeDuration);
            //apply face
            ApplyFade(_current, t, true);
            yield return null;
        }

        //at the end make the whole text statically hidden
        SetAlpha(_current, 0f);
        _state = State.Idle;
    }

    /// <summary>
    /// Based on the time and state it fades the text from left to right
    /// </summary>
    /// <param name="target">text that should be fading in or out</param>
    /// <param name="t">time of the transition</param>
    /// <param name="fadeOut">if true, it's fading out else it's fading in</param>
    private void ApplyFade(TMP_Text target, float t, bool fadeOut)
    {
        //FROM DOCS:
        //ForceMeshUpdate -> Function to force regeneration of the mesh before its normal process time. 
        //This is useful when changes to the text object properties need to be applied immediately.
        target.ForceMeshUpdate();
        //retrieves the textInfo. Class which contains information about every element contained within the text object.
        TMP_TextInfo textInfo = target.textInfo;

        float minX = float.MaxValue, maxX = float.MinValue;
        //for each character in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            //retrieve the info
            TMP_CharacterInfo c = textInfo.characterInfo[i];
            //if it's not visible then we need to skip it
            if (!c.isVisible) continue;

            //retrieve the left corner of the first character and the right corner of the last character to get the dimension
            if (c.bottomLeft.x < minX) minX = c.bottomLeft.x;
            if (c.topRight.x > maxX) maxX = c.topRight.x;
        }

        //calculate the width of the text
        float textWidth = maxX - minX;
        if (textWidth <= 0f) return;

        //the cusp of the gradient that is moved based on time passed in the transition
        float front = t * (1f + _gradientWidth) - _gradientWidth;

        //for each character in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            //retrieve the info
            TMP_CharacterInfo c = textInfo.characterInfo[i];
            if (!c.isVisible) continue;

            //center that is used to know if the character should be faded
            float charCenter = ((c.bottomLeft.x + c.topRight.x) * 0.5f - minX) / textWidth;
            //clamp the alpha based on the distance between the character center from the gradient front and gradient size
            float bandProgress = Mathf.Clamp01((charCenter - front) / _gradientWidth);
            float alpha = fadeOut ? bandProgress : (1f - bandProgress);

            //set the alpha
            SetCharAlpha(textInfo, i, alpha);
        }

        //foreach mesh in the text info
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            //set the alpha transparency
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.colors32 = meshInfo.colors32;
            //update the geometry
            target.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    /// <summary>
    /// Sets the alpha of a single character
    /// </summary>
    /// <param name="textInfo">TextInfo of the parent text</param>
    /// <param name="charIndex">Index of the character inside the text</param>
    /// <param name="alpha">Alpha target for the single character</param>
    private void SetCharAlpha(TMP_TextInfo textInfo, int charIndex, float alpha)
    {
        //retrieve the info regarding the single character
        TMP_CharacterInfo c = textInfo.characterInfo[charIndex];
        //get mesh and vertexes (gets the index of the first one)
        int mesh = c.materialReferenceIndex;
        int vertex = c.vertexIndex;

        //get the color of the mesh
        Color32[] colors = textInfo.meshInfo[mesh].colors32;
        byte a = (byte)(alpha * 255f);

        //update all the subsequent vertexes the constitute the character
        colors[vertex + 0].a = a;
        colors[vertex + 1].a = a;
        colors[vertex + 2].a = a;
        colors[vertex + 3].a = a;
    }

    /// <summary>
    /// Set the alpha value for a whole text
    /// </summary>
    /// <param name="target">Text target</param>
    /// <param name="alpha">alpha target</param>
    private void SetAlpha(TMP_Text target, float alpha)
    {
        target.ForceMeshUpdate();
        TMP_TextInfo textInfo = target.textInfo;

        //for each character
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            //if it's visible set the alpha of that character to be the chosen value
            if (textInfo.characterInfo[i].isVisible)
                SetCharAlpha(textInfo, i, alpha);
        }

        //for each mesh
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {   
            //update the color and update the geometry
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            target.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    /// <summary>
    /// Returns whether the first text has already been displayed.
    /// </summary>
    /// <returns>true if it's the first text (text object empty), false otherwise</returns>
    public bool IsFirstText()
    {
        return _textA.text.Equals("");
    }
}