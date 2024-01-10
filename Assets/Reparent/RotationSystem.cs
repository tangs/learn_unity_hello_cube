using Common;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Reparent
{
    public partial struct RotationSystem : ISystem
    {
        private EntityQuery _rotationCubesQuery;
        private RotationJob _rotationJob;

        private ComponentTypeHandle<RotationSpeedData> _rotationHandle;
        private ComponentTypeHandle<LocalTransform> _transformHandle;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<RotationSpeedData>();
            state.RequireForUpdate<Common.Reparent>();

            _rotationCubesQuery = SystemAPI.QueryBuilder().WithAll<
                RotationSpeedData,
                LocalTransform
            >().Build();
            _rotationJob = new RotationJob();

            _rotationHandle = state.GetComponentTypeHandle<RotationSpeedData>(isReadOnly: true);
            _transformHandle = state.GetComponentTypeHandle<LocalTransform>(isReadOnly: false);
        }

        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;

            _rotationHandle.Update(ref state);
            _transformHandle.Update(ref state);
            
            _rotationJob.deltaTime = deltaTime;
            _rotationJob.rotationHandle = _rotationHandle;
            _rotationJob.transformHandle = _transformHandle;
            
            state.Dependency = _rotationJob.Schedule(_rotationCubesQuery, state.Dependency);
        }
    }

    internal struct RotationJob : IJobChunk
    {
        [ReadOnly]
        public ComponentTypeHandle<RotationSpeedData> rotationHandle;
        public ComponentTypeHandle<LocalTransform> transformHandle;

        public float deltaTime;
        
        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, 
            bool useEnabledMask, in v128 chunkEnabledMask)
        {
            // Assert.IsFalse(useEnabledMask);

            var rotations = chunk.GetNativeArray(ref rotationHandle);
            var transforms = chunk.GetNativeArray(ref transformHandle);

            var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out var i))
            {
                transforms[i] = transforms[i].RotateY(rotations[i].radiansPerSecond * deltaTime);
            }
        }
    }
}