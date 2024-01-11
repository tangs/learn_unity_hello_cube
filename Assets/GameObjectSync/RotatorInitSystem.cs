using Common;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GameObjectSync
{
    public partial struct RotatorInitSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.GameObjectSync>();
            state.RequireForUpdate<DirectoryManaged>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var directoryManaged = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();

            foreach (var (_, entity) in SystemAPI.Query<RotationSpeedData>()
                         .WithNone<RotatorGameObject>()
                         .WithEntityAccess())
            {
                var gameObject = Object.Instantiate(directoryManaged.rotatorPrefab);
                ecb.AddComponent(entity, new RotatorGameObject
                {
                    gameObject = gameObject,
                });
            }
            
            ecb.Playback(state.EntityManager);
        }
    }

    public class RotatorGameObject : IComponentData
    {
        public GameObject gameObject;
    }
}