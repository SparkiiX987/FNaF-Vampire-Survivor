using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private List<Wave> waves = new();

    private int currentWaveIndex = 0;

    [SerializeField]
    private List<EnemyPool> enemiesPossible = new();

    [SerializeField]
    private float remainingWaveTime;

    [SerializeField]
    private MapBounds mapBounds;

    [SerializeField]
    private Transform enemiesParent;

    public static event Action<Transform> AddActiveEnemy;
    public static event Action<Transform> RemoveActiveEnemy;

    private void Start()
    {
        InitPools();

        StartCoroutine(ProcessWaveSpawn());
    }

    private void InitPools()
    {
        foreach (EnemyPool enemyPool in enemiesPossible)
        {
            enemyPool.InitPool(enemiesParent);
        }
    }

    private IEnumerator ProcessWaveSpawn()
    {
        Vector3 spawnPos = new Vector3();
        while (currentWaveIndex < waves.Count)
        {
            remainingWaveTime = waves[currentWaveIndex].waveTime;

            WaitForSeconds waveSpawnInterval = new WaitForSeconds(waves[currentWaveIndex].spawnCooldown);

            while (remainingWaveTime > 0)
            {
                spawnPos.Set(Random.Range(mapBounds.mapStartPoint.y, mapBounds.mapSize.y), 0, Random.Range(mapBounds.mapStartPoint.x, mapBounds.mapSize.y));

                SpawnEnemy(spawnPos);

                remainingWaveTime -= waves[currentWaveIndex].spawnCooldown;
                yield return waveSpawnInterval;
            }

            currentWaveIndex++;

            yield return new WaitForSeconds(5);
        }
    }

    private void SpawnEnemy(Vector3 _spawnPosition)
    {
        int randomEnemyIndex = Random.Range(0, waves[currentWaveIndex].enemies.Count - 1);

        ObjectPool<GameObject> currentPool = GetPoolFromEnemyType(waves[currentWaveIndex].enemies[randomEnemyIndex]);

        if(currentPool == null)
        {
            return;
        }

        GameObject enemy = currentPool.Get();

        enemy.transform.position = _spawnPosition;
    }

    private ObjectPool<GameObject> GetPoolFromEnemyType(EnemyType _enemyType)
    {
        foreach (EnemyPool enemyPool in enemiesPossible)
        {
            if (enemyPool.enemyFactory.enemyType == _enemyType)
            {
                return enemyPool.pool;
            }
        }

        return null;
    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemies = new();

        [SerializeField]
        public float waveTime;

        [SerializeField]
        public float spawnCooldown;
    }

    [System.Serializable]
    public class EnemyPool
    {
        public EnemyStatsFactoryPair enemyFactory;
        public GameObject enemyPrefab;
        public ObjectPool<GameObject> pool;

        public float health;
        public float movementSpeed;
        public float damages;
        public float attackRange;
        public int experienceGived;
        public float attackCooldown;

        public Material material;

        public void InitPool(Transform enemiesParent, int _defaultCapacity = 25, int _maxSize = 300)
        {
            pool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject newEnemy = Instantiate(enemyPrefab, enemiesParent);
                    newEnemy.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;
                    AIBehaviour ai = newEnemy.GetComponent<AIBehaviour>();
                    ai.pool = pool;
                    newEnemy.SetActive(false);
                    return newEnemy;
                },
                (GameObject _pooledObject) =>
                {
                    _pooledObject.SetActive(true);
                    AddActiveEnemy?.Invoke(_pooledObject.transform);
                    CreateStatsWithFactory(enemyFactory.factoryType, _pooledObject.GetComponent<AIBehaviour>());
                },
                (GameObject _pooledObject) =>
                {
                    RemoveActiveEnemy?.Invoke(_pooledObject.transform);
                    _pooledObject.SetActive(false);
                },
                (GameObject _pooledObject) => Destroy(_pooledObject),
                true, _defaultCapacity, _maxSize);
        }


        private void CreateStatsWithFactory(FactoryType _factoryType, AIBehaviour _enemy)
        {
            switch (_factoryType)
            {
                case FactoryType.Base:
                    AIBaseStatsFactory factory = new();

                    _enemy.SetStats(factory.CreateNewStatsTable()
                        .SetHealthPoint(health)
                        .SetDamages(damages)
                        .SetMovementSpeed(movementSpeed)
                        .SetAttackRange(attackRange)
                        .SetExperienceGived(experienceGived)
                        .SetAttackCooldown(attackCooldown)
                        .BuildStats());

                    break;
            }
        }
    }
}

//[System.Serializable]
//public class MapBounds
//{
//    public Vector2 XBounds;
//    public Vector2 YBounds;
//}

[System.Serializable]
public class EnemyStatsFactoryPair
{
    public EnemyType enemyType;
    public FactoryType factoryType;
}

public enum FactoryType
{
    Base,
}

public enum EnemyType
{
    Base = 0,
    Intermediary = 1,
    Big = 2,
    Boss = 3,
}
