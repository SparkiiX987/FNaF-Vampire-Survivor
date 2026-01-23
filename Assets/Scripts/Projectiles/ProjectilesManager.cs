using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilesManager : MonoBehaviour
{
    [SerializeField]
    private List<ProjectilPool> projectilPools = new List<ProjectilPool>();

    [SerializeField]
    private Transform projectilParent;

    private void Start()
    {
        InitPools();

        PlayerController.fireProjectil += SpawnProjectil;
    }

    private void OnDestroy()
    {
        PlayerController.fireProjectil -= SpawnProjectil;
    }

    private void InitPools()
    {
        foreach (ProjectilPool projectilPool in projectilPools)
        {
            projectilPool.InitPool(projectilParent);
        }
    }

    private void SpawnProjectil(Vector3 _spawnPosition, projectilType _projectilType, Vector3 _dir, float _damages)
    {
        ObjectPool<GameObject> currentPool = GetPoolFromEnemyType(_projectilType);

        GameObject projectil = currentPool.Get();

        projectil.transform.position = _spawnPosition;

        projectil.GetComponent<Projectile>().Initialize(_dir, _damages);
    }

    private ObjectPool<GameObject> GetPoolFromEnemyType(projectilType _projectilType)
    {
        foreach (ProjectilPool projectilPool in projectilPools)
        {
            if (projectilPool.projectilType == _projectilType)
            {
                return projectilPool.pool;
            }
        }

        return null;
    }

    [System.Serializable]
    class ProjectilPool
    {
        public GameObject projectilPool;
        public ObjectPool<GameObject> pool;
        public projectilType projectilType;

        public void InitPool(Transform _projectilParent, int _defaultCapacity = 25, int _maxSize = 300)
        {
            pool = new ObjectPool<GameObject>(() =>
            {
                GameObject newEnemy = Instantiate(projectilPool, _projectilParent);
                Projectile projectile = newEnemy.GetComponent<Projectile>();
                projectile.pool = pool;
                newEnemy.SetActive(false);
                return newEnemy;
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

public enum projectilType : uint
{
    playerAutoAttack = 0,
}