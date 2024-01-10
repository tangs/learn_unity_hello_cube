using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace EnableComponents
{
    [BurstCompile]
    public partial struct RotationSystem : ISystem
    {
        private const float Interval = 3.0f;

        private float _delayTime;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.EnableComponent>();
            state.RequireForUpdate<RotationSpeed>();

            _delayTime = Interval;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            _delayTime -= deltaTime;
            
            if (_delayTime < 0f)
            {
                foreach (var rotationSpeed in SystemAPI
                             .Query<EnabledRefRW<RotationSpeed>>()
                             .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState))
                {
                    rotationSpeed.ValueRW = !rotationSpeed.ValueRO;
                }
                
                _delayTime += Interval;
            }

            foreach (var (rotationSpeed, transform) in SystemAPI
                         .Query<RefRO<RotationSpeed>, RefRW<LocalTransform>>())
            {
                var transformValue = transform.ValueRO;
                var angle = rotationSpeed.ValueRO.radiusPerSecond * deltaTime;
                transform.ValueRW = transformValue.RotateY(angle);
            }
        }
    }
}