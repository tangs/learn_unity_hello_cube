using System.Linq;
using Common;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace JobEntity
{
    public partial struct SpawnSystem : ISystem
    {
        private float _spawnDelaySecond;
        private uint _randomIndex;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawn>();
            state.RequireForUpdate<Common.JobEntity>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            _spawnDelaySecond -= deltaTime;
            
            if (_spawnDelaySecond > 0f) return;

            // calc next spawn delay time.
            var spawn = SystemAPI.GetSingleton<Spawn>();
            _spawnDelaySecond += spawn.periodSecond;

            var random = Random.CreateFromIndex(_randomIndex++);
            var entities = state.EntityManager.Instantiate(
                spawn.entity, spawn.count, Allocator.Temp);

            var half = new float3(0.5f, 0.5f, 0.5f);
            var color = new float4(random.NextFloat3(), 1f);
            
            foreach (var entity in entities)
            {
                var transform = 
                    SystemAPI.GetComponentRW<LocalTransform>(entity);
                var baseColor = 
                    SystemAPI.GetComponentRW<URPMaterialPropertyBaseColor>(entity);
                
                transform.ValueRW.Position = (random.NextFloat3() - half) * spawn.range;
                baseColor.ValueRW.Value = color;
            }
            
        }
    }
}