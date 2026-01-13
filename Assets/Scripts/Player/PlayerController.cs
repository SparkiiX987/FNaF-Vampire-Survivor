using System;
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
    private Transform meshTransform;

    private Transform mapTransform;
    private Vector2 moveDir;

    private float currentAACooldown;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform projectileSpawnPoint;

    public static event Action<float, float> UpdateHealthBar;

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
        if (moveDir != Vector2.zero)
        {
            mapTransform.position += stats.GetMovementSpeed * Time.deltaTime * new Vector3(moveDir.x, transform.position.y, moveDir.y);
        }

        RotatePlayer();

        if (currentAACooldown <= 0)
        {
            FireAutoAttack();
        }
        else
        {
            currentAACooldown -= Time.deltaTime;
        }
    }

    private void FireAutoAttack()
    {
        animator.SetTrigger("FireAA");
        currentAACooldown = stats.GetAttackCooldown;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);

        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 dir = (raycastHit.point - transform.position).normalized;
            dir.y = 0;
            projectile.GetComponent<Projectile>().Initialize(dir, stats.GetDamages);
        }
    }

    private void RotatePlayer()
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            meshTransform.LookAt(new Vector3(raycastHit.point.x, transform.position.y, raycastHit.point.z));
        }
    }

    public void SetMapTransform(Transform _mapTransform)
    {
        mapTransform = _mapTransform;
    }

    private void StartMove(InputAction.CallbackContext _ctx)
    {
        moveDir = (_ctx.ReadValue<Vector2>()) * -1;
        animator.SetBool("IsIdle", false);
    }

    private void Stop(InputAction.CallbackContext _ctx)
    {
        moveDir = Vector2.zero;
        animator.SetBool("IsIdle", true);
    }

    public void TakeDamages(float _amount)
    {
        stats.SetHealth(stats.GetCurrentHealth - _amount);

        if (stats.GetCurrentHealth <= 0)
        {
            stats.SetHealth(0);
            OnDeath();
        }

        UpdateHealthBar.Invoke(stats.GetCurrentHealth, stats.GetMaxHealth);
    }

    private void OnDeath()
    {
        print("mort");
    }

    public void SetPlayerStats(PlayerStats _newStats)
    {
        stats = _newStats;
    }
}
