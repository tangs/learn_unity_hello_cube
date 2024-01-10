using Common;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Reparent
{
    public partial struct ReparentSystem : ISystem
    {
        private const float Interval = 2.5f;
        private bool _attached;
        private float _delayTimeSeconds;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.Reparent>();
            state.RequireForUpdate<RotationSpeedData>();

            _attached = true;
            _delayTimeSeconds = Interval;
        }

        public void OnUpdate(ref SystemState state)
        {
            _delayTimeSeconds -= SystemAPI.Time.DeltaTime;
            if (_delayTimeSeconds > 0f) return;

            _delayTimeSeconds += Interval;

            var rotationEntity = SystemAPI.GetSingletonEntity<RotationSpeedData>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            if (_attached)
            {
                foreach (var child in SystemAPI.GetBuffer<Child>(rotationEntity))
                {
                    ecb.RemoveComponent<Parent>(child.Value);
                }
            }
            else
            {
                foreach (var (_, entity) in SystemAPI.Query<RefRO<LocalTransform>>()
                             .WithNone<RotationSpeedData>().WithEntityAccess())
                {
                    ecb.AddComponent(entity, new Parent
                    {
                        Value = rotationEntity
                    });
                }
            }

            ecb.Playback(state.EntityManager);
            _attached = !_attached;
        }
    }
}