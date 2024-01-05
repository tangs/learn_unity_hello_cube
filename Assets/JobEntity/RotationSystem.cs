using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace JobEntity
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.JobEntity>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var job = new MyJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = (float)SystemAPI.Time.ElapsedTime,
            };
            job.Schedule();
        }
    }

    [BurstCompile]
    internal partial struct MyJob : IJobEntity
    {
        public float deltaTime;
        public float elapsedTime;

        private void Execute(
            ref LocalTransform transform, 
            ref PostTransformMatrix matrix, 
            Common.RotationSpeedData rotationSpeed)
        {
            transform = transform.RotateY(rotationSpeed.radiansPerSecond * deltaTime);
            matrix.Value = float4x4.Scale(1, math.sin(elapsedTime), 1);
        }
    }
}