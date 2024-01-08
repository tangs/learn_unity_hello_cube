using System.Linq;
using Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Prefabs
{
    [BurstCompile]
    public partial struct SpawnSystem : ISystem
    {
        private uint _updateCount;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawner>();
            state.RequireForUpdate<Common.Prefabs>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var data = SystemAPI.QueryBuilder().WithAll<RotationSpeedData>().Build();
            if (data.IsEmpty is false) return;

            var prefab = SystemAPI.GetSingleton<Spawner>().prefab;
            var random = Random.CreateFromIndex(_updateCount++);
            
            var instances = state.EntityManager.Instantiate(prefab, 500, Allocator.Temp);
            
            foreach (var entity in instances)
            {
                var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                transform.ValueRW.Position = (random.NextFloat3() - new float3(0.5f, 0f, 0.5f)) * 20f;
            }
        }
    }
}