using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;

    private InputSystem_Actions inputSystem;
    private InputAction moveInput;

    private float currentAACooldown;

    [SerializeField]
    private PlayerExperienceManager experienceManager;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Transform meshTransform;

    private Transform mapTransform;
    private Vector2 moveDir;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform projectileSpawnPoint;

    public static event Action<float, float> UpdateHealthBar;
    public static event Action<float> UpdateExperienceBar;

    public static event Action<Vector3, projectilType, Vector3, float> fireProjectil;

    [SerializeField] private List<Spell> spells = new List<Spell>();

    private LineRenderer laser;
    private Coroutine laserAnimationCoroutine;

    [SerializeField]
    private List<AnimationCurve> laserCurves;
    
    [SerializeField]
    private Gradient laserColor;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();

        moveInput = inputSystem.Player.Move;

        /*laser = GetComponent<LineRenderer>();*/

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

        experienceManager.InitLevels();
        UpdateExperienceBar.Invoke(experienceManager.GetExperienceAvancement());
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
        if(Time.timeScale == 0)
        {
            return;
        }

        if (moveDir != Vector2.zero)
        {
            mapTransform.position += stats.GetMovementSpeed * Time.deltaTime * new Vector3(moveDir.x, mapTransform.position.y, moveDir.y);
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

        ProcessSpells();
    }

    private void ProcessSpells()
    {
        foreach(Spell spell in spells)
        {
            spell.currentCooldown -= Time.deltaTime;

            if( spell.currentCooldown <= 0 )
            {
                spell.Use(transform);
            }
        }
    }

    private void FireAutoAttack()
    {
        if(fireProjectil == null)
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 dir = new();
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            dir = (raycastHit.point - transform.position).normalized;
            dir.y = 0;
        }

        fireProjectil.Invoke(projectileSpawnPoint.position, projectilType.playerAutoAttack, dir, stats.GetDamages);
        currentAACooldown = stats.GetAttackCooldown;
    }

    public void FireLaser(float _laserCooldown, Vector3 _enemyPosition)
    {
        if(laser == null)
        {
            GameObject laserGameObject = new GameObject();
            laserGameObject.transform.position = Vector3.zero;
            laserGameObject.name = "laser";
            laser = laserGameObject.AddComponent<LineRenderer>();
            laser.colorGradient = laserColor;
        }

        if(laserAnimationCoroutine != null)
            { StopCoroutine(laserAnimationCoroutine); }

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, _enemyPosition);

        laserAnimationCoroutine = StartCoroutine(LaserAnimation(_laserCooldown));
    }

    private IEnumerator LaserAnimation(float _laserCooldown)
    {
        WaitForSeconds wait = new WaitForSeconds(_laserCooldown / 10f);

        laser.enabled = true;

        for(int i = 0; i < 10; i++)
        {
            laser.widthCurve = laserCurves[i];
            yield return wait;
        }

        laser.enabled = false;
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
    }

    private void Stop(InputAction.CallbackContext _ctx)
    {
        moveDir = Vector2.zero;
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
        SceneManager.LoadScene(0);
    }

    public void SetPlayerStats(PlayerStats _newStats)
    {
        stats = _newStats;
    }

    public int GetLevel()
    {
        return experienceManager.level;
    }

    public float GetExperienceAvancement()
    {
        return experienceManager.GetExperienceAvancement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out ExperienceOrbe orbe))
        {
            experienceManager.AddExperience(orbe.GetExperiences());
            UpdateExperienceBar.Invoke(experienceManager.GetExperienceAvancement());
            orbe.PickUpOrbe();
        }
    }

    public void AddUpgradeComponent(PlayerComponent _component)
    {
        if(_component is Passif passif)
        {
            AddStats(passif);
        }
        else if (_component is Spell spell)
        {
            spells.Add(spell);
        }
        else
        {
            Debug.LogError($"Error : The component {_component.componentName} is neither a Passif or a Spell.");
        }
    }

    private void AddStats(Passif _passif)
    {
        foreach(StatGived stat in _passif.statsGived)
        {
            switch(stat.statType)
            {
                case Statsname.Health:
                    stats.SetMaxHealth(stats.GetMaxHealth + stat.amount);
                    UpdateHealthBar.Invoke(stats.GetCurrentHealth, stats.GetMaxHealth);
                    break;
                case Statsname.Damages:
                    stats.SetDamages(stats.GetDamages + stat.amount);
                    break;
                case Statsname.MouvementSpeed:
                    stats.SetMovementSpeed(stats.GetMovementSpeed + stat.amount);
                    break;
                case Statsname.AttackCooldown:
                    stats.SetAttackCooldown(stats.GetAttackCooldown - stat.amount);
                    break;
                case Statsname.HealthPassiveRegen:
                    stats.SetHealthPassiveRegen(stats.GetHealthPassiveRegen + stat.amount);
                    break;
                case Statsname.LifeSteal:
                    stats.SetLifeSteal(stats.GetLifeSteal + stat.amount);
                    break;
            }
        }
    }
}
