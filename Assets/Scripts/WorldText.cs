using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
    private WorldObject _objectOrigin;
    [SerializeField]
    private DecalProjector _decal;

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
    //[SerializeField]
    //    private Sprite _hand, _leftFoot, _rightFoot;

    [SerializeField]
    private TMP_Text _text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timePassed = 0;
    }

    /// <summary>
    /// Sets up the text with all the informations about the object it describes and how the player touched it (hand or feet)
    /// </summary>
    public void Setup(WorldObject obj, PrintType print)
    {
        this._objectOrigin = obj;
        this._text.text = obj.Text();

        HandlePrint(print);
        //ApplyDecalRotation(decalNormal);
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

    public void ApplyDecalRotation(Vector3 decalNormal, Transform cameraTransform)
    {
        Vector3 decalUp = Vector3.ProjectOnPlane(Vector3.up, decalNormal).normalized;
        if (decalUp.sqrMagnitude < 0.001f)
            decalUp = Vector3.ProjectOnPlane(cameraTransform.forward, decalNormal).normalized;

        _decal.transform.rotation = Quaternion.LookRotation(decalNormal, decalUp);

        // Same flip the text uses — keeps the decal's up aligned with the player's up
        if (Vector3.Dot(_decal.transform.up, cameraTransform.up) < 0)
            _decal.transform.Rotate(0, 0, 180f);
        } 

    private void HandlePrint(PrintType print)
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
        this._decal.material = print == PrintType.Left? GameAssets.i.LeftFootMaterial : (print == PrintType.Right? GameAssets.i.RightFootMaterial : GameAssets.i.HandMaterial);
      //  if (print == PrintType.Hand)
      //      this._decal.transform.Rotate(Vector3.forward, Random.Range(-100, 100));
        
    }
}
