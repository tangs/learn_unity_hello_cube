using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Common
{
    public class RotationSpeedAuthoring : MonoBehaviour
    {
        public float degree;
        
        private class Baker : Baker<RotationSpeedAuthoring>
        {
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic | TransformUsageFlags.NonUniformScale);
                AddComponent(entity, new RotationSpeedData
                {
                    radiansPerSecond =  math.radians(authoring.degree)
                });
            }
        }
    }

    public struct RotationSpeedData : IComponentData
    {
        public float radiansPerSecond;
    }
}
