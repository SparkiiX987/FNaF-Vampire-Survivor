using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Transform playerStart;

    [SerializeField] 
    private Transform map;

    private PlayerStatsFactory playerStatsFactory = new();

    private static GameMode gameMode;

    public static GameObject playerRef;

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

        PlayerController pc = playerRef.GetComponent<PlayerController>();

        pc.SetPlayerStats(playerStats);
        pc.SetMapTransform(map);
    }

    public static GameMode Get => gameMode;
}
