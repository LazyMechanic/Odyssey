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
            GameObject beatshipPrefab = GetBeatshipPrefab();
            GameObject beatshipInstance = InstantiateBeatship(beatshipPrefab);

            CreateBeatshipEntity(beatshipInstance);
        }

        GameObject GetBeatshipPrefab()
        {
            GameObject beatshipPrefab = Resources.Load<GameObject>("Beatship/Beatship");
            Assert.IsNotNull(beatshipPrefab, "Beatship prefab in resources not found ");

            return beatshipPrefab;
        }

        GameObject InstantiateBeatship(GameObject prefab)
        {
            GameObject beatshipInstance = GameObject.Instantiate(prefab, _beatshipSpawnFilter.Components2[0].transform.position, Quaternion.identity);
            beatshipInstance.name = "Beatship";

            return beatshipInstance;
        }

        void CreateBeatshipEntity(GameObject beatshipInstance)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BeatshipViewRadiusComponent>(out BeatshipViewRadiusComponent viewRadius)
                         .AddComponent<BeatshipAltitudeComponent>(out BeatshipAltitudeComponent altitude)
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<RigidbodyComponent>(out RigidbodyComponent rigidbody)
                         .AddComponent<PidComponent>(out PidComponent pid)
                         .AddComponent<BeatshipTagComponent>()
                         .AddComponent<AntiGravityComponent>()
                         .AddComponent<PidValueComponent>()
                         .AddComponent<SpeedComponent>();

            transform.transform = beatshipInstance.transform;

            rigidbody.rigidbody = beatshipInstance.GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody.rigidbody, "Beatship rigidbody not found");

            pid.kp = 10.0f;
            pid.ki = 0.0f;
            pid.kd = 1.0f;

            altitude.defaultAltitude = 4.0f;

            viewRadius.viewRadius = 250.0f;

            BeatshipBehaviour beatshipBehaviour = beatshipInstance.GetComponent<BeatshipBehaviour>();
            Assert.IsNotNull(beatshipBehaviour, "Beatship behaviour script on beatship prefab not found");

            beatshipBehaviour.viewRadius = viewRadius.viewRadius;
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