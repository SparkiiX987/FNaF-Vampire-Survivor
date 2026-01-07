using UnityEngine;
using UnityEngine.Pool;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    private Stats stats;

    private Transform playerTransform;

    private IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool { set => pool = value; }

    void Start()
    {

    }

    public void SetStats(Stats _stats)
    {
        stats = _stats;
    }

    public void TakeDamages(float _amount)
    {
        stats.SetHealth(stats.GetCurrentHealth - _amount);

        if (stats.GetCurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            return;
        }

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        Vector3 dir = (playerTransform.position - transform.position).normalized;

        transform.position = stats.GetMovementSpeed * Time.deltaTime * dir;
    }

    public void SetPlayerTransform(Transform _playerTransform)
    {
        playerTransform = _playerTransform;
    }

    private void OnDeath()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
