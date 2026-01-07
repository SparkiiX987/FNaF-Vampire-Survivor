using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private List<Wave> waves = new();

    private int currentWaveIndex = 0;

    [SerializeField]
    private List<EnemyPool> enemiesPossible = new();

    [SerializeField]
    private float remainingWaveTime;

    private void Start()
    {
        
    }

    private IEnumerator ProcessWaveSpawn()
    {
        while(remainingWaveTime > 0)
        {

            yield return new WaitForSeconds(waves[currentWaveIndex].spawnCooldown);
        }


    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, waves[currentWaveIndex].enemies.Count);

        ObjectPool<GameObject> currentPool = GetPoolFromEnemyType(waves[currentWaveIndex].enemies[randomEnemyIndex]);

        GameObject enemy = currentPool.Get();
    }

    private ObjectPool<GameObject> GetPoolFromEnemyType(EnemyType _enemyType)
    {
        foreach(EnemyPool enemyPool in enemiesPossible)
        {
            if(enemyPool.enemy == _enemyType)
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
        public EnemyType enemy;
        public GameObject enemyPrefab;
        public ObjectPool<GameObject> pool;

        public int health;
        public int movementSpeed;
        public int damages;

        public EnemyPool(int _defaultCapacity, int _maxSize)
        {
            pool = new ObjectPool<GameObject>(
                () => 
                {
                    GameObject newEnemy = Instantiate(enemyPrefab);
                    newEnemy.GetComponent<AIBehaviour>().Pool = pool;
                    return Instantiate(enemyPrefab);
                },
                (GameObject pooledObject) => pooledObject.SetActive(true),
                (GameObject pooledObject) => pooledObject.SetActive(false),
                (GameObject pooledObject) => Destroy(pooledObject),
                true, _defaultCapacity, _maxSize);
        }
    }
}


public enum EnemyType
{
    MeleeCreep = 0,
}
