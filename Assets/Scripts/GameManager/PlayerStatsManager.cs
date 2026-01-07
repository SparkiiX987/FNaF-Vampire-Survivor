using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float atk;
    [SerializeField] private float atks;
    [SerializeField] private float regen;

    private static PlayerStatsManager instance;

    public static PlayerStatsManager Get => instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetSpeed()
    {
        return speed; 
    }

    public float GetDamages()
    {
        return atk;
    }

    public float GetAttaqueCooldown()
    {
        return atks;
    }

    public float GetRegen()
    {
        return regen;
    }
}
