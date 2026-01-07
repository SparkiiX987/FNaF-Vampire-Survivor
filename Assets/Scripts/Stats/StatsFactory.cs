using UnityEngine;

public abstract class StatsFactory<TFactory, TStats>
    where TFactory : StatsFactory<TFactory, TStats>
    where TStats : Stats, new()
{
    protected TStats stats;

    public TFactory CreateNewStatsTable()
    {
        stats = new TStats();
        return (TFactory)this;
    }

    public TFactory CopyStats(TStats _stats)
    {
        stats = _stats;
        return (TFactory)this;
    }

    public TFactory SetHealthPoint(float value)
    {
        stats.SetMaxHealth(value);
        return (TFactory)this;
    }

    public TFactory SetMovementSpeed(float value)
    {
        stats.SetMovementSpeed(value);
        return (TFactory)this;
    }

    public TFactory SetDamages(float value)
    {
        stats.SetDamages(value);
        return (TFactory)this;
    }

    public TStats BuildStats()
    {
        return stats;
    }
}


public class PlayerStatsFactory : StatsFactory<PlayerStatsFactory, PlayerStats>
{
    public PlayerStatsFactory SetAttackCooldown(float _newHealth)
    {
        stats.SetAttackCooldown(_newHealth);
        return this;
    }

    public PlayerStatsFactory SetHealthPassiveRegen(float _newHealth)
    {
        stats.SetHealthPassiveRegen(_newHealth);
        return this;
    }
}
