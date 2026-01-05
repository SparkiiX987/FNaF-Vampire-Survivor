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

    void Update()
    {
        if(playerTransform == null)
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
        pool.Release(gameObject);
    }
}
