using Common;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace MainThread
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.MainThread>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (transform, speed) in 
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeedData>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(
                    speed.ValueRO.radiansPerSecond * deltaTime);
            }
        }
    }
}