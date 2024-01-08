using Unity.Burst;
using Unity.Entities;

namespace JobEntity
{
    public partial struct SpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Spawn>();
            state.RequireForUpdate<Common.JobEntity>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
        }
    }
}