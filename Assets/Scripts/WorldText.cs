using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
    private WorldObject _objectOrigin;

    [SerializeField]
    private float _life;

    [SerializeField]
    private float _timePassed;

    [SerializeField]
    private float _fadeInTime = 0.3f;

    [SerializeField]
    private Image _print;

    private Animator _animator;

    private PrintType _printType;

    /// <summary>
    /// I'd like to use the fucking game assets but right now it's not working
    /// </summary>
    [SerializeField]
    private Sprite _hand, _leftFoot, _rightFoot;

    private Sprite[] _prints;

    [SerializeField]
    private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timePassed = 0;
        _prints = new Sprite[]
        {
            _leftFoot,
            _rightFoot,
            _hand
        };
    }

    /// <summary>
    /// Sets up the text with all the informations about the object it describes and how the player touched it (hand or feet)
    /// </summary>
    public void Setup(WorldObject obj, PrintType print)
    {
        this._objectOrigin = obj;
        this._text.text = obj.Text();

        HandlePrint();
       
        
    }

    public void FixedUpdate()
    {
        this._timePassed += Time.fixedDeltaTime;
        //fast fade in, then slow fade out
        if (_timePassed < _fadeInTime)
        {
            this._text.color = new Color(1, 1, 1, _timePassed / _fadeInTime);            
        } else
        {
            this._text.color = new Color(1, 1, 1, 1f - (_timePassed / _life));    
        }

        if (_timePassed >= _life)
            Destroy(this.gameObject);
    } 

    private void HandlePrint()
    {
        /* CANVAS VERSION */
        /*
         //set the correct print type sprite
        this._printType = print;
        //fuckiung shit i cant fuicking believe it
        this._print.sprite = print == PrintType.Left? _leftFoot : (print == PrintType.Right? _rightFoot : _hand);

        //rotate randomly the handprint
        if (print == PrintType.Hand)
            this._print.transform.Rotate(Vector3.forward, Random.Range(-100, 100));
        */

        /* DECAL VERSION */
        
    }
}
