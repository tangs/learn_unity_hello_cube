using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace JobEntity
{
    public class SpawnAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public int cubeCount;
        public Vector3 range;
        public float periodSecond;
        
        private class Baker : Baker<SpawnAuthoring>
        {
            public override void Bake(SpawnAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Spawn
                {
                    entity = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                    count = authoring.cubeCount,
                    range = authoring.range,
                    periodSecond = authoring.periodSecond,
                });
            }
        }
    }

    internal struct Spawn : IComponentData
    {
        public Entity entity;
        public int count;
        public float3 range;
        public float periodSecond;
    }
}