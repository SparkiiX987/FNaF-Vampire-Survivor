using System;
using UnityEngine;
using UnityEngine.Pool;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    public EnemyStats stats;

    private float currentAttackCooldown;

    private bool isDead;

    public IObjectPool<GameObject> pool;

    [SerializeField]
    public static event Action<Vector3, int> SpawnExperienceOrbe;

    private void OnEnable()
    {
        isDead = false;
    }

    public void SetStats(EnemyStats _stats)
    {
        stats = _stats;
    }

    public void TakeDamages(float _amount)
    {
        stats.SetHealth(stats.GetCurrentHealth - _amount);

        if (stats.GetCurrentHealth <= 0)
        {
            stats.SetHealth(0);
            OnDeath();
        }
    }

    public void Attack(Transform _playerTransform)
    {
        currentAttackCooldown = stats.GetAttackCooldown;
        _playerTransform.GetComponent<PlayerController>().TakeDamages(GetComponentInParent<AIBehaviour>().stats.GetDamages);
    }

    public void AiAttack(Transform _playerTransform)
    {
        if (!_playerTransform) return;

        Vector3 delta = _playerTransform.position - transform.position;
        float sqrDist = delta.sqrMagnitude;
        float sqrAttackRange = stats.GetAttackRange * stats.GetAttackRange;

        bool isInRange = sqrDist <= sqrAttackRange;

        if (isInRange && currentAttackCooldown <= 0)
        {
            Attack(_playerTransform);
            return;
        }

        currentAttackCooldown -= Time.deltaTime;
    }

    private void OnDeath()
    {
        if (pool != null && !isDead)
        {
            if(SpawnExperienceOrbe != null)
            { 
                SpawnExperienceOrbe.Invoke(transform.position, stats.GetExperienceGived);
            }
            isDead = true;
            pool.Release(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
