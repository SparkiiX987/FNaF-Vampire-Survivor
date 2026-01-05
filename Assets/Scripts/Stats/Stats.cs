using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] 
    private float movementSpeed;

    [SerializeField]
    private float currentHealt;

    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float damages;

    public float GetMovementSpeed => movementSpeed;
    public float GetCurrentHealt => currentHealt;
    public float GetMaxHealth => maxHealth;
    public float GetDamages => damages;
}
