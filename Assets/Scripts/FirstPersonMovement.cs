using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private CharacterController characterController;
    Vector2 velocity;
    
    private Vector3 movementInput;
    
    // Start is called before the first frame update
    void Start()
    {
        movementInput = Vector2.zero;
    }


    void Update()
    {
        Vector3 movement = movementInput.x * transform.right + movementInput.z * transform.forward;
        characterController.Move(movement * speed * Time.deltaTime);
    }

    public void OnMoveInput(InputAction.CallbackContext context) {
        movementInput = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }
}
