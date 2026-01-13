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

    public TFactory CopyStats(TStats _value)
    {
        stats = _value;
        return (TFactory)this;
    }

    public TFactory SetHealthPoint(float _value)
    {
        stats.SetMaxHealth(_value);
        return (TFactory)this;
    }

    public TFactory SetMovementSpeed(float _value)
    {
        stats.SetMovementSpeed(_value);
        return (TFactory)this;
    }

    public TFactory SetDamages(float _value)
    {
        stats.SetDamages(_value);
        return (TFactory)this;
    }

    public TFactory SetAttackRange(float _value)
    {
        stats.SetAttackRange(_value);
        return (TFactory)this;
    }

    public TStats BuildStats()
    {
        return stats;
    }
}


public class PlayerStatsFactory : StatsFactory<PlayerStatsFactory, PlayerStats>
{
    public PlayerStatsFactory SetAttackCooldown(float _value)
    {
        stats.SetAttackCooldown(_value);
        return this;
    }

    public PlayerStatsFactory SetHealthPassiveRegen(float _value)
    {
        stats.SetHealthPassiveRegen(_value);
        return this;
    }
}

public class AIBaseStatsFactory : StatsFactory<AIBaseStatsFactory, Stats>
{

}
