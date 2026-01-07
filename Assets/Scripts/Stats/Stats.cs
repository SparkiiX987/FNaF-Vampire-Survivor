using UnityEngine;

public class Stats
{
    protected float movementSpeed;

    protected float currentHealth;

    protected float maxHealth;

    protected float damages;


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
}

public class PlayerStats : Stats
{
    protected float attackCooldown;

    protected float healthPassiveRegen;


    public float GetAttackCooldown => attackCooldown;
    public float GetHealthPassiveRegen => healthPassiveRegen;


    public void SetAttackCooldown(float _attackCooldown)
    {
        attackCooldown = _attackCooldown;
    }

    public void SetHealthPassiveRegen(float _healthPassiveRegen)
    {
        healthPassiveRegen = _healthPassiveRegen;
    }
}
