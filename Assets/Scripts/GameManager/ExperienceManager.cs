using UnityEngine;
using UnityEngine.Pool;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField]
    private GameObject orbePrefab;

    [SerializeField]
    private ExperiencePool experiencePool = new();

    [SerializeField]
    private Transform projectilParent;

    Vector3 orbeSpawnPoint = Vector3.zero;

    private void Start()
    {
        InitPool();

        AIBehaviour.SpawnExperienceOrbe += SpawnExperience;
    }

    private void OnDestroy()
    {
        AIBehaviour.SpawnExperienceOrbe -= SpawnExperience;
    }

    private void InitPool()
    {
        experiencePool.InitPool(orbePrefab, projectilParent);
    }

    private void SpawnExperience(Vector3 _spawnPosition, int experiences)
    {
        GameObject expOrb = experiencePool.pool.Get();

        orbeSpawnPoint.Set(_spawnPosition.x, 0.25f, _spawnPosition.z);

        expOrb.transform.position = orbeSpawnPoint;

        expOrb.GetComponent<ExperienceOrbe>().InitOrbe(experiences);
    }

    class ExperiencePool
    {
        public GameObject orbe;
        public ObjectPool<GameObject> pool;
        public projectilType projectilType;

        public void InitPool(GameObject _prefab, Transform _experienceParent, int _defaultCapacity = 25, int _maxSize = 300)
        {
            orbe = _prefab;

            pool = new ObjectPool<GameObject>(() =>
            {
                GameObject newOrbe = Instantiate(this.orbe, _experienceParent);
                ExperienceOrbe orbe = newOrbe.GetComponent<ExperienceOrbe>();
                orbe.pool = pool;
                newOrbe.SetActive(false);
                return newOrbe;
            },
            (GameObject _pooledObject) =>
            {
                _pooledObject.SetActive(true);
            },
            (GameObject _pooledObject) =>
            {
                _pooledObject.SetActive(false);
            },
            (GameObject _pooledObject) => Destroy(_pooledObject),
            true, _defaultCapacity, _maxSize);
        }

    }
}
