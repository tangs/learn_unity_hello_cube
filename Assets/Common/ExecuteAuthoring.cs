using Unity.Entities;
using UnityEngine;

namespace Common
{
    public class ExecuteAuthoring : MonoBehaviour
    {
        public bool mainThread;
        public bool iJobEntity;
        public bool aspects;
        public bool prefabs;
        public bool reparent;
        public bool enableComponent;

        private class Baker : Baker<ExecuteAuthoring>
        {
            public override void Bake(ExecuteAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                if (authoring.mainThread) AddComponent<MainThread>(entity);
                if (authoring.iJobEntity) AddComponent<JobEntity>(entity);
                if (authoring.aspects) AddComponent<Aspects>(entity);
                if (authoring.prefabs) AddComponent<Prefabs>(entity);
                if (authoring.reparent) AddComponent<Reparent>(entity);
                if (authoring.enableComponent) AddComponent<EnableComponent>(entity);
            }
        }

    }

    public struct MainThread : IComponentData
    {
    }

    public struct JobEntity : IComponentData
    {
    }
    
    public struct Aspects : IComponentData
    {
    }
    
    public struct Prefabs : IComponentData
    {
    }
    
    public struct Reparent : IComponentData
    {
    }
    
    public struct EnableComponent : IComponentData
    {
    }
}
