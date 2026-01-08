using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Transform playerStart;

    private PlayerStatsFactory playerStatsFactory = new();

    public static GameObject playerRef {  get; private set; }


    private void Awake()
    {
        playerRef = Instantiate(player, playerStart.position, playerStart.rotation);

        PlayerStatsManager playerStatsManager = PlayerStatsManager.Get;

        PlayerStats playerStats = playerStatsFactory.CreateNewStatsTable()
            .SetMovementSpeed(playerStatsManager.GetSpeed())
            .SetHealthPoint(playerStatsManager.GetHealth())
            .SetDamages(playerStatsManager.GetDamages())
            .SetAttackCooldown(playerStatsManager.GetAttaqueCooldown())
            .SetHealthPassiveRegen(playerStatsManager.GetRegen())
            .BuildStats();

        playerRef.GetComponent<PlayerController>().SetPlayerStats(playerStats);
    }
}
