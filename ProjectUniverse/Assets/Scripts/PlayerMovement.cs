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

    public float jumpForce;

    [SerializeField] private Camera cam;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        
        
    }

    
    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        
        
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraRight = cam.transform.right;
        
        cameraForward.y = 0;
        cameraForward.Normalize();
    

        _moveDir = (cameraForward * input.y + cameraRight * input.x).normalized;
        
        rb.linearVelocity = _moveDir * moveSpeed;
        rb.angularVelocity = Vector3.up * jumpForce;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * jumpForce);
        
    }

    private void OnEnable()
    {
        jumpAction.action.performed += Jump;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= Jump;
    }


   
}
