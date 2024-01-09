using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Assertions;

namespace JobEntity
{
    [BurstCompile]
    public partial class RotationSystem : SystemBase
    {
        private ComponentTypeHandle<PostTransformMatrix> _postTransformHandle;
        private ComponentTypeHandle<LocalTransform> _transformHandle;
        private ComponentTypeHandle<Common.RotationSpeedData> _rotationSpeedHandle;
        
        [BurstCompile]
        protected override void OnCreate()
        {
            RequireForUpdate<Common.JobEntity>();
    
            _postTransformHandle = GetComponentTypeHandle<PostTransformMatrix>();
            _transformHandle = GetComponentTypeHandle<LocalTransform>();
            _rotationSpeedHandle = GetComponentTypeHandle<Common.RotationSpeedData>(true);
        }
        
        [BurstCompile]
        protected override void OnUpdate()
        {
            var spinningCubesQuery = SystemAPI.QueryBuilder().WithAll<
                Common.RotationSpeedData, 
                PostTransformMatrix,
                LocalTransform>().Build();
            
            _postTransformHandle.Update(this);
            _transformHandle.Update(this);
            _rotationSpeedHandle.Update(this);
            
            var job = new JobChunk
            {
                postTransformHandle = _postTransformHandle,
                transformHandle = _transformHandle,
                rotationSpeedHandle = _rotationSpeedHandle,
                deltaTime = SystemAPI.Time.DeltaTime,
                elapsedTime = (float)SystemAPI.Time.ElapsedTime,
            };
    
            Dependency = job.ScheduleParallel(spinningCubesQuery, Dependency);
        }
    }
    
    // [BurstCompile]
    // public partial struct RotationSystem : ISystem
    // {
    //     
    //     [BurstCompile]
    //     public void OnCreate(ref SystemState state)
    //     {
    //         state.RequireForUpdate<Common.JobEntity>();
    //     }
    //
    //     [BurstCompile]
    //     public void OnUpdate(ref SystemState state)
    //     {
    //         // var job = new MyJob
    //         // {
    //         //     deltaTime = SystemAPI.Time.DeltaTime,
    //         //     elapsedTime = (float)SystemAPI.Time.ElapsedTime,
    //         // };
    //         // // job.ScheduleParallel();
    //         // job.Schedule();
    //         
    //         var spinningCubesQuery = SystemAPI.QueryBuilder().WithAll<
    //             Common.RotationSpeedData, 
    //             PostTransformMatrix,
    //             LocalTransform>().Build();
    //         
    //         var job = new JobChunk
    //         {
    //             postTransformHandle = SystemAPI.GetComponentTypeHandle<PostTransformMatrix>(),
    //             transformHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(),
    //             rotationSpeedHandle = SystemAPI.GetComponentTypeHandle<Common.RotationSpeedData>(true),
    //             deltaTime = SystemAPI.Time.DeltaTime,
    //             elapsedTime = (float)SystemAPI.Time.ElapsedTime,
    //         };
    //         
    //         state.Dependency = job.ScheduleParallel(spinningCubesQuery, state.Dependency);
    //     }
    // }

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
    
    [BurstCompile]
    internal struct JobChunk : IJobChunk
    {
        public ComponentTypeHandle<PostTransformMatrix> postTransformHandle;
        public ComponentTypeHandle<LocalTransform> transformHandle;
        [ReadOnly]
        public ComponentTypeHandle<Common.RotationSpeedData> rotationSpeedHandle;
        
        public float deltaTime;
        public float elapsedTime;
        
        [BurstCompile]
        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex,
            bool useEnabledMask, in v128 chunkEnabledMask)
        {
            Assert.IsFalse(useEnabledMask);
            
            var matrix = chunk.GetNativeArray(ref postTransformHandle);
            var transforms = chunk.GetNativeArray(ref transformHandle);
            var rotationSpeeds = chunk.GetNativeArray(ref rotationSpeedHandle);

            var enumerator = new ChunkEntityEnumerator(false, chunkEnabledMask, chunk.Count);
            while (enumerator.NextEntityIndex(out var i))
            // for (int i = 0, count = chunk.Count; i < count; ++i)
            {
                transforms[i] = transforms[i].RotateY(rotationSpeeds[i].radiansPerSecond * deltaTime);
                matrix[i] = new PostTransformMatrix
                {
                    Value = float4x4.Scale(1, math.sin(elapsedTime), 1),
                };
            }
        }
    }
}