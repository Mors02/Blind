using TMPro;
using UnityEngine;

public class WorldText : MonoBehaviour
{
    private WorldObject _objectOrigin;

    [SerializeField]
    private float _life;

    [SerializeField]
    private float _timePassed;

    [SerializeField]
    private float _fadeInTime = 0.3f;

    private Animator _animator;

    [SerializeField]
    private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timePassed = 0;        
    }

    /// <summary>
    /// Sets up the text with all the informations about it
    /// </summary>
    public void Setup(WorldObject obj)
    {
        this._objectOrigin = obj;
        this._text.text = obj.Text();        
    }

    public void FixedUpdate()
    {
        this._timePassed += Time.fixedDeltaTime;
        if (_timePassed < _fadeInTime)
        {
            this._text.color = new Color(1, 1, 1, _timePassed / _fadeInTime);            
        } else
        {
            this._text.color = new Color(1, 1, 1, 1f - (_timePassed / _life));    
        }
        Debug.Log(1f - (_timePassed / _life));
        Debug.Log(this._text.color);
        if (_timePassed >= _life)
            Destroy(this.gameObject);
    } 
}
