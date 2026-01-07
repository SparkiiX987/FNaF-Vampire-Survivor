using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerStats stats;

    private InputSystem_Actions inputSystem;
    private InputAction moveInput;

    [SerializeField] 
    private Animator animator;

    [SerializeField] 
    private Camera playerCamera;

    [SerializeField] 
    Transform meshTransform;

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
        print($"Health : {stats.GetCurrentHealth} \n" +
            $" MaxHealth : {stats.GetMaxHealth} \n" +
            $"MS : {stats.GetMovementSpeed} \n" +
            $"Damages : {stats.GetDamages} \n" +
            $"Regen : {stats.GetHealthPassiveRegen} \n" +
            $"AS : {stats.GetAttackCooldown} \n");
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

        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            meshTransform.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }
    }

    private void StartMove(InputAction.CallbackContext _ctx)
    {
        moveDir = _ctx.ReadValue<Vector2>();
        animator.SetTrigger("StartMoving");
        animator.SetBool("IsIdle", false);
    }

    private void Stop(InputAction.CallbackContext _ctx)
    {
        moveDir = Vector2.zero;
        animator.SetBool("IsIdle", true);
    }

    public void SetPlayerStats(PlayerStats _newStats)
    {
        stats = _newStats;
    }
}
