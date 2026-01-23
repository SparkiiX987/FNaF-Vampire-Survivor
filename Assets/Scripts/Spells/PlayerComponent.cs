using UnityEngine;

public class PlayerComponent : ScriptableObject
{
    public string componentName;
    public string description;
    public Rarity rarity;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epique,
    Legendary
}