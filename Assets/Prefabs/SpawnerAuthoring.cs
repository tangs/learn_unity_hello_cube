using Unity.Entities;
using UnityEngine;

namespace Prefabs
{
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        
        private class Baker : Baker<SpawnerAuthoring>
        {
            public override void Bake(SpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Spawner
                {
                    prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
    
    internal struct Spawner : IComponentData
    {
        public Entity prefab;
    }
}