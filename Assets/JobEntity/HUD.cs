using System;
using Common;
using TMPro;
using Unity.Entities;
using UnityEngine;

namespace JobEntity
{
    public class HUD : MonoBehaviour
    {
        private EntityManager _entityManager;
        private EntityQuery _queryRotation;
        
        public TextMeshProUGUI textCount;

        public void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _queryRotation = _entityManager.CreateEntityQuery(
                ComponentType.ReadOnly<RotationSpeedData>()
            );
        }

        public void Update()
        {
            var elementCnt = _queryRotation.CalculateEntityCount();
            textCount.text = $"count: {elementCnt}";
        }
    }
}
