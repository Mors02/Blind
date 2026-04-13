using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f, _mouseSensitivity = 4f;

    [SerializeField]
    private Camera _mainCamera;

    private Vector3 _currentMovement;
    private float _verticalRotation;

    private Rigidbody _rb;
    private CharacterController _cc;

    [SerializeField]
    private Transform _cameraTransform;
    private Vector2 _moveInput, _lookInput;

    [SerializeField]
    private InputActionAsset _playerControls;

    private InputAction _moveAction;
    private InputAction _touchAction;
    private InputAction _interactAction;
    private InputAction _lookAction;

    [SerializeField]
    private LayerMask _touchLayer, _interactLayer, _floorLayer;


    private Vector3 _from, _to, _normal;

    [SerializeField]
    private GameObject _worldTextPrefab;

    private Vector3 _lastFootprint;

    [Range(1f, 10f)]
    [SerializeField]
    private float _footSpacer = 4f;
    private PrintType _whichFoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CharacterController>();
        
        _moveAction = _playerControls.FindActionMap("Player").FindAction("Move");
        _moveAction.performed += OnMove;
        _moveAction.canceled += StopMovement;

        _touchAction = _playerControls.FindActionMap("Player").FindAction("Touch");
        _touchAction.performed += OnTouch;
        
        _interactAction = _playerControls.FindActionMap("Player").FindAction("Interact");
        _interactAction.performed += OnInteract;
        
        /*_lookAction = _playerControls.FindActionMap("Player").FindAction("Look");
        _lookAction.performed += OnLook;
        _lookAction.canceled += StopLook;*/

        _whichFoot = PrintType.Left;
        _lastFootprint = this.transform.position;

        //Debug.Log(GameManager.i.State);
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _touchAction.Enable();
        _interactAction.Enable();
//        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _touchAction.Disable();
        _interactAction.Disable();
      //  _lookAction.Disable();
    }

    public void FixedUpdate()
    {
        HandleMovement();
        HandleFootprints();
        
    }
    #region Movement and controls
    /// <summary>
    /// Handles the movement and the creation of footprints
    /// </summary>
    private void HandleMovement()
    {
        float horizontalSpeed = _moveInput.x * _speed;
        float verticalSpeed = _moveInput.y * _speed;

        Vector3 horizontalMovement = new Vector3 (horizontalSpeed, 0, verticalSpeed);

        horizontalMovement = _cameraTransform.rotation * horizontalMovement;
        _currentMovement.x = horizontalMovement.x;
        _currentMovement.z = horizontalMovement.z;
        //if the player is moving
        if (_currentMovement.magnitude > 0 && GameManager.i.State == StateMachineStep.Inspect)
        {   
            //close all menus
            GameManager.ChangeState(StateMachineStep.Free);
        }

        _cc.Move(_currentMovement * Time.fixedDeltaTime);
        
    }

    private void HandleLook()
    {
        float mouseXRotation = _lookInput.x;
        transform.Rotate(0, mouseXRotation, 0);

        _verticalRotation -= _lookInput.y;
        
        this.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
    }

    private void HandleFootprints()
    {
        //if it's moving
        if (_currentMovement.magnitude > 0)
        {
            float distanceSinceLastFootprint = Vector3.Distance(_lastFootprint, this.transform.position);
            //Debug.Log(distanceSinceLastFootprint);
            if (distanceSinceLastFootprint >= _footSpacer)
            {
                Vector3 from = _normal = this.transform.position;
                //raycast bit in front to make it more visible
                //if (Physics.Raycast(from, Vector3.ProjectOnPlane(_cameraTransform.forward*0.15f, -_cameraTransform.up), out RaycastHit hit, 3f, _touchLayer))
                if (Physics.Raycast(from, -transform.up, out RaycastHit hit, 3f, _floorLayer))
                {   
                    //only if its walkable then create the footprint
                    if (hit.collider.CompareTag("Floor"))
                    {
                        //instantiate the text on the step
                        GameObject text = InstantiateText(hit, _whichFoot);
                        //update what was last step
                        _lastFootprint = text.transform.position;
                        //invert what was the last foot
                        _whichFoot = _whichFoot == PrintType.Left? PrintType.Right : PrintType.Left;    
                    }
                }
                
            }
        }
    }
    #endregion

    #region Actions
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void StopMovement(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }

   public void OnTouch(InputAction.CallbackContext context)
    {
        //_from = new Vector3(transform.position.x, transform.position.y + transform.localScale.y/2, transform.position.z);
        _from = _cameraTransform.position;
        //Check that the ray hits and we are free to move
        if (Physics.Raycast(_from, _cameraTransform.forward, out RaycastHit hit, 5f, _touchLayer) && GameManager.i.State == StateMachineStep.Free)
        {   
            InstantiateText(hit);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //_from = new Vector3(transform.position.x, transform.position.y + transform.localScale.y/2, transform.position.z);
        _from = _cameraTransform.position;
        //interact only with a text
        if (Physics.Raycast(_from, _cameraTransform.forward, out RaycastHit hit, 5f, _interactLayer) && GameManager.i.State == StateMachineStep.Free)
        {
            //if it hits
            if (hit.collider != null)
            {                
                if (hit.collider.gameObject.TryGetComponent(out WorldText text))
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    //enter inspect state in the stace machine
                    if (hit.collider.gameObject.CompareTag("Reflect"))
                    {
                        GameManager.ChangeState(StateMachineStep.Inventory);
                    }
                    else
                    {
                        GameManager.i.DialogueEvents.EnterDialogue(text.GetObjectKnot());
                        GameManager.ChangeState(StateMachineStep.Inspect);
                    }
                    
                }
                
            }
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void StopLook(InputAction.CallbackContext context)
    {
        _lookInput = Vector2.zero;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(_from, _from + _cameraTransform.forward * 5f);
        //Gizmos.DrawCube(_lastFootprint, new Vector3(0.5f, 0.5f, 0.5f));
    }

    #region Text instantiation section
    /// <summary>
    /// When touching or walking this spawns a prefab that contains the print and the text. By default, spawns an hand
    /// </summary>
    /// <param name="hit">The point of contact with a surface</param>
    /// <param name="type">The type of print to be spawned</param>
    private GameObject InstantiateText(RaycastHit hit, PrintType type = PrintType.Hand)
    {
        WorldObject obj = hit.collider.gameObject.GetComponent<WorldObject>();

        GameObject worldText = Instantiate(_worldTextPrefab, hit.point, Quaternion.identity);
        worldText.GetComponent<WorldText>().Setup(obj, type);
        //Vector3 normal = hit.normal; // always points toward camera
        Vector3 normal = hit.normal; //GetDecalNormal(hit);

        // Text's Z = surface normal → flush to surface, facing player
        // Text's Y = world up projected onto surface → stable, no spiral
        Vector3 textUp = Vector3.ProjectOnPlane(Vector3.up, normal).normalized;

        // Fallback for floor / ceiling (world up is parallel to normal)
        if (textUp.sqrMagnitude < 0.001f)
            textUp = Vector3.ProjectOnPlane(_cameraTransform.forward, normal).normalized;

        worldText.transform.rotation = Quaternion.LookRotation(normal, textUp);

        // Flip if upside down relative to camera
        if (Vector3.Dot(worldText.transform.up, _cameraTransform.up) < 0)
            worldText.transform.Rotate(0, 0, 180f);

        Vector3 decalNormal = GetDecalNormal(hit);
        worldText.GetComponent<WorldText>().ApplyDecalRotation(decalNormal, _cameraTransform);

        return worldText;
    }

    /// <summary>
    /// Returns hit.normal, unless the decal footprint crosses a 90° edge —
    /// in which case returns the normalised average of the two face normals,
    /// so the decal bisects the corner cleanly.
    /// </summary>
    private Vector3 GetDecalNormal(RaycastHit hit)
    {
        const float halfSize = 0.5f;   // must match DecalProjector m_Size / 2
        const float epsilon  = 0.03f;
        int mask = Physics.DefaultRaycastLayers;

        Vector3 n = hit.normal;

        Vector3 t = Vector3.Cross(n, Vector3.up).normalized;
        if (t.sqrMagnitude < 0.001f)
            t = Vector3.Cross(n, Vector3.forward).normalized;
        Vector3 b = Vector3.Cross(t, n).normalized;

        foreach (Vector3 dir in new[] { t, -t, b, -b })
        {
            // Probe at the decal boundary — if nothing below, we're past an edge
            Vector3 probe = hit.point + n * epsilon + dir * halfSize;
            if (Physics.Raycast(probe, -n, halfSize, mask, QueryTriggerInteraction.Ignore))
                continue;

            // Find the adjacent face
            Vector3 searchOrigin = hit.point + dir * (halfSize + epsilon) - n * epsilon;
            if (!Physics.Raycast(searchOrigin, -dir, out RaycastHit adj,
                    halfSize + 0.5f, mask, QueryTriggerInteraction.Ignore))
                continue;

            bool isConvex = Vector3.Dot(adj.normal, n) < 0;
            // Confirm it's a genuine 90° edge (normals are perpendicular)
            // if (Vector3.Dot(adj.normal, n) > 0.1f)
            //    continue;
            Debug.Log(isConvex);
            Debug.Log(
                adj.normal);
            Vector3 adjNormal = new Vector3(Mathf.Abs(adj.normal.x), Mathf.Abs(adj.normal.y), Mathf.Abs(adj.normal.z));//Vector3.Dot(adj.normal, n) < 0 ? -adj.normal : adj.normal;
            // ← Blend and return; no second decal needed
            return (n + adjNormal).normalized;
        }

        return n; // no edge nearby, use surface normal as-is
    }
    #endregion
}

public enum PrintType
{
    Left,
    Right,
    Hand
}