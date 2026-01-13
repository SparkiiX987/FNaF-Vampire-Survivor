using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

public struct AIForcesJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float2> flowDirections;
    [ReadOnly] public NativeArray<float> flowForce;
    [ReadOnly] public NativeArray<float> AiVelocity;
    public NativeArray<float2> forcesToAdd;

    public void Execute(int index)
    {
            forcesToAdd[index] += flowDirections[index] * AiVelocity[index] * flowForce[index];
    }
}