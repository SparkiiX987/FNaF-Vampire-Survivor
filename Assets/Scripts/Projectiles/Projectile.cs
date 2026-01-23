using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeSpan;
    public float damages;

    public IObjectPool<GameObject> pool;

    private bool isAlive;

    Vector3 avancementFactor;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterLifeSpan());
        isAlive = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DisableAfterLifeSpan()
    {
        yield return new WaitForSeconds(lifeSpan);
        isAlive = false;
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void Initialize(Vector3 _dir, float _damages)
    {
        damages = _damages;
        avancementFactor = _dir.normalized * speed;
    }

    private void Update()
    {
        transform.position += avancementFactor * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isAlive)
        {
            return;
        }

        isAlive = false;

        if (!other.transform.TryGetComponent(out AIBehaviour aiBehaviour))
        {
            DestroyProjectile();
            return;
        }

        aiBehaviour.TakeDamages(damages);

        DestroyProjectile();
    }
}
