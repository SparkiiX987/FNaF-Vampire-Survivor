using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passif", menuName = "Augmentation/Passif")]
public class Passif : PlayerComponent
{
    public List<StatGived> statsGived = new List<StatGived>();
}

[System.Serializable]
public class StatGived
{
    public Statsname statType;
    public float amount;
}

public enum Statsname
{
    Health,
    Damages,
    MouvementSpeed,
    AttackCooldown,
    HealthPassiveRegen,
    LifeSteal,
}