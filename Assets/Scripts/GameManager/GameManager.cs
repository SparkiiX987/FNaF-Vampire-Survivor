using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region FlowFieldVars
    [SerializeField]
    private FlowFieldManager flowFieldManager;

    [SerializeField]
    private float flowFieldRefreshTimer;
    #endregion

    #region AiForcesCalculVar
    NativeArray<float2> forcesToAdd;
    NativeArray<float2> dirs;
    NativeArray<float> forces;
    NativeArray<float> velocities;

    JobHandle forceCalculatorHandle;

    List<Transform> enemies = new List<Transform>();
    List<AIBehaviour> enemyAIs = new();

    List<Transform> pendingEnnemiesToAdd = new();
    List<Transform> pendingEnnemiesToRemove = new();

    bool isJobRunning;
    #endregion

    public int EnemiesCount => enemies.Count;

    private void Start()
    {
        WaveManager.AddActiveEnemy += AddActiveEnemy;
        WaveManager.RemoveActiveEnemy += RemoveActiveEnemy;

        StartCoroutine(RecalculFlowField());
    }

    private void OnDestroy()
    {
        WaveManager.AddActiveEnemy -= AddActiveEnemy;
        WaveManager.RemoveActiveEnemy -= RemoveActiveEnemy;
    }

    private void Update()
    {
        if (enemies.Count == 0) return;

        forceCalculatorHandle.Complete();

        forcesToAdd = new NativeArray<float2>(enemies.Count, Allocator.TempJob);

        GetEnemiesFlowFieldInfos();
        GetEnemiesVelocities();

        AIForcesJob job = new AIForcesJob
        {
            flowDirections = dirs,
            flowForce = forces,
            AiVelocity = velocities,
            forcesToAdd = forcesToAdd
        };

        isJobRunning = true;
        forceCalculatorHandle = job.Schedule(enemies.Count, 32);
    }

    private void LateUpdate()
    {
        if (!forcesToAdd.IsCreated)
            return;

        forceCalculatorHandle.Complete();
        isJobRunning = false;

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].position +=
                new Vector3(forcesToAdd[i].x, 0, forcesToAdd[i].y) * Time.deltaTime;
        }

        AddAllPending();
        RemoveAllPending();

        if (dirs.IsCreated) dirs.Dispose();
        if (forces.IsCreated) forces.Dispose();
        if (velocities.IsCreated) velocities.Dispose();
        if (forcesToAdd.IsCreated) forcesToAdd.Dispose();
    }

    private void GetEnemiesFlowFieldInfos()
    {
        dirs = new NativeArray<float2>(enemies.Count, Allocator.TempJob);
        forces = new NativeArray<float>(enemies.Count, Allocator.TempJob);

        int index = 0;

        foreach (Transform enemy in enemies)
        {
            if (flowFieldManager.TryGetCellFromWorld(enemy.position, out Cell cell))
            {
                print($"{enemy.position} -> {cell.cellPosition}");
                dirs[index] = cell.direction;
                forces[index] = cell.force;
            }

            index++;
        }
    }

    private void GetEnemiesVelocities()
    {
        velocities = new NativeArray<float>(enemies.Count, Allocator.TempJob);

        for (int i = 0; i < velocities.Length; i++)
        {
            velocities[i] = enemyAIs[i].stats.GetMovementSpeed;
        }
    }

    private IEnumerator RecalculFlowField()
    {
        WaitForSeconds timer = new WaitForSeconds(flowFieldRefreshTimer);
        yield return null;

        while (true)
        {
            flowFieldManager.RecalculFlowField();

            yield return timer;
        }
    }

    private void AddActiveEnemy(Transform _transform)
    {
        if (isJobRunning)
        {
            pendingEnnemiesToAdd.Add(_transform);
            return;
        }

        enemies.Add(_transform);
        enemyAIs.Add(_transform.GetComponent<AIBehaviour>());
    }

    private void RemoveActiveEnemy(Transform _transform)
    {
        if (isJobRunning)
        {
            pendingEnnemiesToRemove.Add(_transform);
            return;
        }

        enemies.Remove(_transform);
        enemyAIs.Remove(_transform.GetComponent<AIBehaviour>());
    }

    private void AddAllPending()
    {
        foreach (Transform aiTransform in pendingEnnemiesToAdd)
        {
            enemies.Add(aiTransform);
            enemyAIs.Add(aiTransform.GetComponent<AIBehaviour>());
        }

        pendingEnnemiesToAdd.Clear();
    }

    private void RemoveAllPending()
    {
        foreach (Transform aiTransform in pendingEnnemiesToRemove)
        {
            enemies.Remove(aiTransform);
            enemyAIs.Remove(aiTransform.GetComponent<AIBehaviour>());
        }

        pendingEnnemiesToRemove.Clear();
    }
}