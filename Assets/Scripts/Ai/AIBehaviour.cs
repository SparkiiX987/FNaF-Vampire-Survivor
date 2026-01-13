using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIBehaviour : MonoBehaviour
{
    [SerializeField]
    public Stats stats { get; private set; }

    public IObjectPool<GameObject> pool;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private List<AnimationAttackEventTime> attackAnim;

    [SerializeField]
    private float loopCooldownDuration;

    public void SetStats(Stats _stats)
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

    public void TickAI(Transform _playerTransform)
    {
        if (!_playerTransform) return;

        Vector3 delta = _playerTransform.position - transform.position;
        float sqrDist = delta.sqrMagnitude;
        float sqrAttackRange = stats.GetAttackRange * stats.GetAttackRange;

        bool isInRange = sqrDist <= sqrAttackRange;

        if (animator.GetBool("Attacking") != isInRange)
            animator.SetBool("Attacking", isInRange);

        if (isInRange) return;
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

    public void TakeDamages(int _amount)
    {
        stats.SetHealth(stats.GetCurrentHealth -  _amount);

        if(stats.GetCurrentHealth <= 0)
        {
            print("mort");
        }
    }
}

[System.Serializable]
class AnimationAttackEventTime
{
    public AnimationClip animationClip;
    public float Time;
}
