using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace EnableComponents
{
    public class RotationSpeedAuthoring : MonoBehaviour
    {
        public float degreePerSecond;
        public bool enableWhenStart;

        private class Baker : Baker<RotationSpeedAuthoring>
        {
            public override void Bake(RotationSpeedAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                AddComponent(entity, new RotationSpeed
                {
                    radiusPerSecond = math.radians(authoring.degreePerSecond)
                });
                SetComponentEnabled<RotationSpeed>(entity, authoring.enableWhenStart);
            }
        }
    }

    public struct RotationSpeed : IEnableableComponent, IComponentData
    {
        public float radiusPerSecond;
    }
}