using Unity.Burst;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace GameObjectSync
{
    [BurstCompile]
    public partial struct DirectoryInitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.GameObjectSync>();
        }

        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;

            var gameObject = GameObject.Find("Directory");
            Assert.IsNotNull(gameObject);

            var directory = gameObject.GetComponent<Directory>();

            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, new DirectoryManaged
            {
                rotatorPrefab = directory.rotatorPrefab,
                rotationToggle = directory.rotationToggle,
            });
        }
    }

    public class DirectoryManaged : IComponentData
    {
        public GameObject rotatorPrefab;
        public Toggle rotationToggle;
    }
}