using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    public float sensitivity = 1;
    //public float smoothing = 2;

    private Vector2 currentMouseLook;
    //private Vector2 appliedMouseDelta;

    [SerializeField] private InputActionReference lookAction;
    private Transform character;
    private Vector2 mouseDelta;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookAction.action.Enable();
        lookAction.action.performed += OnLookPerformed;
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void OnDisable()
    {
        // Disable the "Look" InputAction when the script is disabled.
        lookAction.action.Disable();
    }

    // No need for the Update method in this context

    void OnLookPerformed(InputAction.CallbackContext context)
    {
        // Get mouse look using new Input system.
        mouseDelta = context.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        currentMouseLook += mouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

        // Rotate camera and controller.
        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }
}
