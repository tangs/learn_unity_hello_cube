using Common;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.Aspects>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            foreach (var (transform, speedData) in 
                     SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeedData>>())
            {
                transform.ValueRW = transform.ValueRO.RotateY(speedData.ValueRO.radiansPerSecond * deltaTime);
            }
        }
    }

    internal readonly partial struct VerticalMovementAspect : IAspect
    {
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<RotationSpeedData> _rotationSpeed;

        public void Move(double elapsedTime)
        {
            _transform.ValueRW.Position.y = (float)math.sin(
                _rotationSpeed.ValueRO.radiansPerSecond * elapsedTime);
        }
    }
    
}