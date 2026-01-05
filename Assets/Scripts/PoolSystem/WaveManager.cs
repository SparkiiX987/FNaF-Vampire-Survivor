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

    private void Start()
    {
        
    }

    private void SpawnEnemy()
    {
        int randomEnemyIndex = UnityEngine.Random.Range(0, waves[currentWaveIndex].enemies.Count);


    }

    [System.Serializable]
    public class Wave
    {
        public List<EnemyType> enemies = new();

        [SerializeField]
        public float waveTime;
    }

    [System.Serializable]
    public class EnemyPool
    {
        public EnemyType enemy;
        public GameObject enemyPrefab;
        public ObjectPool<GameObject> pool;

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
    Endo = 0,
    PrototypeFreddy = 1,
    ToyChica = 2,
    ToyFreddy = 3,
    Mangle = 4,
    ToyBonnie = 5,
}
