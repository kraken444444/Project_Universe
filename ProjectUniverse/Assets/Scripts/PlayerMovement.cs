using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    
    public float moveSpeed;
    
    private Vector3 _moveDir;

    public InputActionReference moveAction;



    private void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        _moveDir = new Vector3(input.x, 0, input.y).normalized;

    }


    private void FixedUpdate()
    {
        rb.linearVelocity = _moveDir * moveSpeed;
            
    }
}
