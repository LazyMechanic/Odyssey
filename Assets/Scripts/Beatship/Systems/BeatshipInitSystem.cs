using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipSpawnTagComponent, TransformComponent> _beatshipSpawnFilter = null;

        private EcsFilter<BeatshipTagComponent> _beatshipFilter = null;
        
        void IEcsInitSystem.Initialize ()
        {
            var beatshipPrefab = Resources.Load<GameObject>("Beatship/Beatship");
            Assert.IsNotNull(beatshipPrefab, "Not found beatship prefab in resources");

            var spaceShipInstance = GameObject.Instantiate(beatshipPrefab);

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<RigidbodyComponent>(out RigidbodyComponent rigidbody)
                         .AddComponent<BeatshipAltitudeComponent>(out BeatshipAltitudeComponent altitude)
                         .AddComponent<BeatshipTagComponent>()
                         .AddComponent<AntiGravityComponent>()
                         .AddComponent<PidValueComponent>()
                         .AddComponent<SpeedComponent>()
                         .AddComponent<PidComponent>(out PidComponent pid);

            transform.transform = spaceShipInstance.transform;
            transform.transform.position = _beatshipSpawnFilter.Components2[0].transform.position;

            rigidbody.rigidbody = spaceShipInstance.GetComponent<Rigidbody>();
            
            pid.kp = 10.0f;
            pid.ki = 0.0f;
            pid.kd = 1.0f;

            altitude.defaultAltitude = 4.0f;
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _beatshipFilter)
            {
                _world.RemoveEntity(_beatshipFilter.Entities[i]);
            }
        }
    }
}