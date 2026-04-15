using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WorldText : MonoBehaviour
{
    [SerializeField]
    private WorldObjectInfo _objectInfo;
    private WorldObject _objectOrigin;
    public WorldObject ObjectInteracted {get {return _objectOrigin;}}

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

    [SerializeField]
    [Range(0f, 5f)]
    private float _noRepeatRadius;

    [SerializeField]
    private bool Active = true;

    [SerializeField]
    private LayerMask _textMask;

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

        this.Active = true;

        Collider[] colliders = Physics.OverlapSphere(transform.position, _noRepeatRadius, _textMask);
        Debug.Log(colliders.Length);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out WorldText text) && collider.gameObject != this.gameObject)
            {
                Debug.Log(text.Active + " " + text.ObjectInteracted);
                Debug.Log(this.Active + " " + this.ObjectInteracted);
                //if another text is active in the zone of the same object then do not show
                if (text.Active && text.ObjectInteracted == this.ObjectInteracted)
                    this.Active = false;
            }
        }

        //set the text visible or invisible
        if (_text != null)
            this._text.gameObject.SetActive(Active);
        else
            Debug.LogWarning(name + " has no text selected");
    }

    /// <summary>
    /// Sets up the text with all the informations about the object it describes and how the player touched it (hand or feet)
    /// </summary>
    public void Setup(WorldObject obj, PrintType print)
    {
        this._objectOrigin = obj;
        this._text.text = obj.Text;

        HandlePrint(print);
        //ApplyDecalRotation(decalNormal);
    }

    public void FixedUpdate()
    {
        this._timePassed += Time.fixedDeltaTime;
        if (_text == null)
            return;
        //fast fade in, then slow fade out
        if (_timePassed < _fadeInTime)
        {
            this._text.color = new Color(1, 1, 1, _timePassed / _fadeInTime);            
        } else
        {
            this._text.color = new Color(1, 1, 1, 1f - (_timePassed / _life));    
        }

        if (_life > 0 && _timePassed >= _life)
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
    public string GetObjectKnot()
    {
        if (_objectOrigin == null)
            return _objectInfo.knotId;
        else
            return _objectOrigin.InkKnot;
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
