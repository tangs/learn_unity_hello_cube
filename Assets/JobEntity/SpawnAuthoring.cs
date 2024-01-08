using Unity.Entities;
using UnityEngine;

namespace JobEntity
{
    public class SpawnAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int cubeCount;
        
        private class Baker : Baker<SpawnAuthoring>
        {
            public override void Bake(SpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Spawn
                {
                    count = authoring.cubeCount,
                });
            }
        }
    }

    internal struct Spawn : IComponentData
    {
        public int count;
        public Entity entity;
    }
}