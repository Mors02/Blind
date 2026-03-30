using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5f;

    private Vector3 _currentMovement;

    private Rigidbody _rb;
    private CharacterController _cc;

    [SerializeField]
    private Transform _cameraTransform;
    private Vector2 _moveInput;

    [SerializeField]
    private InputActionAsset _playerControls;

    private InputAction _moveAction;
    private InputAction _touchAction;
    private InputAction _interactAction;

    [SerializeField]
    private LayerMask _touchLayer;

    private Vector3 _from, _to, _normal;

    [SerializeField]
    private GameObject _worldTextPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _cc = GetComponent<CharacterController>();
        _moveAction = _playerControls.FindActionMap("Player").FindAction("Move");
        _moveAction.performed += OnMove;
        _moveAction.canceled += StopMovement;
        _touchAction = _playerControls.FindActionMap("Player").FindAction("Touch");
        _touchAction.performed += OnTouch;
        _interactAction = _playerControls.FindActionMap("Player").FindAction("Interact");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _touchAction.Enable();
        _interactAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _touchAction.Disable();
        _interactAction.Disable();
    }

    public void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontalSpeed = _moveInput.x * _speed;
        float verticalSpeed = _moveInput.y * _speed;

        Vector3 horizontalMovement = new Vector3 (horizontalSpeed, 0, verticalSpeed);

        horizontalMovement = _cameraTransform.rotation * horizontalMovement;
        _currentMovement.x = horizontalMovement.x;
        _currentMovement.z = horizontalMovement.z;
        
        _cc.Move(_currentMovement * Time.fixedDeltaTime);
    }


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
        _from = _cameraTransform.position;

        if (Physics.Raycast(_from, _cameraTransform.forward, out RaycastHit hit, 10f, _touchLayer))
        {
            GameObject worldText = Instantiate(_worldTextPrefab, hit.point, Quaternion.identity);

            Vector3 normal = hit.normal; // always points toward camera

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
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(_to, _to + _normal);
    }
}
