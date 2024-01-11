using Unity.Entities;

namespace GameObjectSync
{
    public partial struct RotationSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Common.GameObjectSync>();
            state.RequireForUpdate<Common.RotationSpeedData>();
            state.RequireForUpdate<DirectoryManaged>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var directoryManaged = SystemAPI.ManagedAPI.GetSingleton<DirectoryManaged>();
            if (directoryManaged.rotationToggle.isOn is false) return;
            
            var rotationSpeed = SystemAPI.GetSingleton<Common.RotationSpeedData>();
            
            var deltaTime = SystemAPI.Time.DeltaTime;
            var radiansPerSecond = rotationSpeed.radiansPerSecond;
            
            foreach (var rotator in SystemAPI.Query<RotatorGameObject>())
            {
                var rotateY = radiansPerSecond * deltaTime;
                rotator.gameObject.transform.Rotate(0f, rotateY, 0f);
            }
        }
    }
}