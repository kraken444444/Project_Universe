using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    
    public float moveSpeed;
    
    private Vector3 _moveDir;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    [SerializeField] public float jumpForce;

    [SerializeField] private Camera cam;

    [SerializeField] private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        
        
    }

    
    private void Update()
    {
        CheckGrounded();
        
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        
        
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        
        cameraForward.y = 0;
        cameraForward.Normalize();
        cameraRight.y = 0;
        cameraRight.Normalize();
    

        _moveDir = (cameraForward * input.y + cameraRight * input.x).normalized;
        
    }

    private void Jump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * jumpForce);
        
    }
    private void FixedUpdate()
    {
        // apply horizontal movement without affecting vertical velocity
        Vector3 horizontalVelocity = _moveDir * moveSpeed;
        Vector3 verticalVelocity = new Vector3(0, rb.linearVelocity.y, 0); 
        rb.linearVelocity = horizontalVelocity + verticalVelocity;
    }

    private void OnEnable()
    {
        jumpAction.action.performed += Jump;
    }
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= Jump;
    }


   
}
