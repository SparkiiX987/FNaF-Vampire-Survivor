using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Stats stats;

    private InputSystem_Actions inputSystem;
    private InputAction moveInput;

    Vector2 moveDir;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();

        moveInput = inputSystem.Player.Move;

        moveInput.performed += StartMove;
        moveInput.canceled += Stop;
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    void Update()
    {
        if(moveDir != Vector2.zero)
        {
            transform.position += stats.GetMovementSpeed * Time.deltaTime * new Vector3(moveDir.x, transform.position.y, moveDir.y);
        }
    }

    private void StartMove(InputAction.CallbackContext _ctx)
    {
        moveDir = _ctx.ReadValue<Vector2>();
    }

    private void Stop(InputAction.CallbackContext _ctx)
    {
        moveDir = Vector2.zero;
    }
}
