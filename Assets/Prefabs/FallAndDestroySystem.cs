using Common;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Prefabs
{
    [BurstCompile]
    public partial struct FallAndDestroySystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<RotationSpeedData>();
            state.RequireForUpdate<Common.Prefabs>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var movement = new float3(0, -SystemAPI.Time.DeltaTime * 5f, 0);
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (transform, entity) in 
                     SystemAPI.Query<RefRW<LocalTransform>>()
                         .WithAll<RotationSpeedData>()
                         .WithEntityAccess())
            {
                transform.ValueRW.Position += movement;
                if (transform.ValueRW.Position.y >= -1f) continue;
                
                ecb.DestroyEntity(entity);
            }
        }
    }
}