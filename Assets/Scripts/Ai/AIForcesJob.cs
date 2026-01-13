using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public struct AIForcesJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> flowDirections;
    [ReadOnly] public NativeArray<float> flowForce;
    [ReadOnly] public NativeArray<float> AiVelocity;
    [ReadOnly] public NativeArray<float3> separationForces;

    public NativeArray<float2> forcesToAdd;

    public void Execute(int index)
    {
        float2 flow = flowDirections[index] * AiVelocity[index] * flowForce[index];

        float2 sep = new float2(separationForces[index].x, separationForces[index].z);

        forcesToAdd[index] = flow + sep;
    }
}

public struct BuildCellMapJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> positions;
    public float cellSize;

    public NativeParallelMultiHashMap<int2, int>.ParallelWriter writer;

    public void Execute(int index)
    {
        int2 cell = new int2(
            (int)math.floor(positions[index].x / cellSize),
            (int)math.floor(positions[index].z / cellSize)
        );

        writer.Add(cell, index);
    }
}

public struct SeparationGridJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> positions;
    [ReadOnly] public NativeParallelMultiHashMap<int2, int> cellMap;

    public float cellSize;
    public float separationRadius;
    public float strength;

    public NativeArray<float3> separationForces;

    public void Execute(int index)
    {
        float3 selfPos = positions[index];
        float3 force = float3.zero;

        int2 cell = new int2(
            (int)math.floor(selfPos.x / cellSize),
            (int)math.floor(selfPos.z / cellSize)
        );

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int2 neighborCell = cell + new int2(x, y);

                if (cellMap.TryGetFirstValue(neighborCell, out int other, out var it))
                {
                    do
                    {
                        if (other == index) continue;

                        float3 diff = selfPos - positions[other];
                        float dist = math.length(diff);

                        if (dist > 0f && dist < separationRadius)
                        {
                            force += math.normalize(diff) * (1f - dist / separationRadius);
                        }

                    } while (cellMap.TryGetNextValue(out other, ref it));
                }
            }
        }

        separationForces[index] = force * strength;
    }
}
