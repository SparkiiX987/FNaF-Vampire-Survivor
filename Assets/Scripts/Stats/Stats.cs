using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField]
    protected float movementSpeed;

    [SerializeField]
    protected float currentHealth;

    [SerializeField]
    protected float maxHealth;

    [SerializeField]
    protected float damages;

    [SerializeField]
    protected float attackCooldown;

    public float GetAttackCooldown => attackCooldown;
    public float GetMovementSpeed => movementSpeed;
    public float GetCurrentHealth => currentHealth;
    public float GetMaxHealth => maxHealth;
    public float GetDamages => damages;


    public void SetMovementSpeed(float _newSpeed)
    {
        movementSpeed = _newSpeed; 
    }

    public void SetMaxHealth(float _maxHealth)
    {
        float diff = _maxHealth - maxHealth;
        maxHealth = _maxHealth;
        currentHealth += diff;
    }

    public void SetHealth(float _health)
    {
        currentHealth = _health;
    }

    public void SetDamages(float _damages)
    {
        damages = _damages;
    }

    public void SetAttackCooldown(float _attackCooldown)
    {
        attackCooldown = _attackCooldown;
        if (attackCooldown <= 0)
        {
            attackCooldown = 0.1f;
        }
    }
}

[System.Serializable]
public class PlayerStats : Stats
{


    [SerializeField]
    protected float healthPassiveRegen;

    [SerializeField]
    protected float lifSteal;

    public float GetHealthPassiveRegen => healthPassiveRegen;
    public float GetLifeSteal => lifSteal;

    public void SetHealthPassiveRegen(float _healthPassiveRegen)
    {
        healthPassiveRegen = _healthPassiveRegen;
    }

    public void SetLifeSteal(float _lifeSteal)
    {
        lifSteal = _lifeSteal;
    }
}

[System.Serializable]
public class EnemyStats : Stats
{
    [SerializeField]
    protected int experienceGived;
    
    [SerializeField]
    protected float attackRange;

    public int GetExperienceGived => experienceGived;
    public float GetAttackRange => attackRange;


    public void SetExperienceGived(int _experienceGived)
    {
        experienceGived = _experienceGived;
    }

    public void SetAttackRange(float _attackRange)
    {
        attackRange = _attackRange;
    }
}