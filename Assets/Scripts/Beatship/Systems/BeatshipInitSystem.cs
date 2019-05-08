using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipTagComponent> _beatshipFilter = null;
        private EcsFilter<BeatshipSpawnerTagComponent, TransformComponent> _beatshipSpawnerFilter = null;
        
        void IEcsInitSystem.Initialize () {
            if (!_beatshipSpawnerFilter.IsEmpty())
            {
                CreateBeatship(_beatshipSpawnerFilter.Components2[0].transform.position);
            }
        }

        void CreateBeatship(Vector3 position)
        {
            GameObject beatshipPrefab = GetBeatshipPrefab();
            GameObject beatshipInstance = InstantiateBeatship(beatshipPrefab, position);
            CreateBeatshipEntity(beatshipInstance);
        }

        GameObject GetBeatshipPrefab()
        {
            GameObject beatshipPrefab = Resources.Load<GameObject>("Beatship/Beatship");
            Assert.IsNotNull(beatshipPrefab, "Beatship prefab in resources not found ");

            return beatshipPrefab;
        }

        GameObject InstantiateBeatship(GameObject prefab, Vector3 position)
        {
            GameObject beatshipInstance = GameObject.Instantiate(prefab, position, Quaternion.identity);
            beatshipInstance.name = "Beatship";

            return beatshipInstance;
        }

        void CreateBeatshipEntity(GameObject beatshipInstance)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BeatshipMaxCollisionAngleComponent>(out BeatshipMaxCollisionAngleComponent maxCollisionAngle)
                         .AddComponent<BeatshipAltitudeComponent>(out BeatshipAltitudeComponent altitude)
                         .AddComponent<BeatshipRotationComponent>(out BeatshipRotationComponent rotation)
                         .AddComponent<BeatshipModelTransform>(out BeatshipModelTransform modelTransform)
                         .AddComponent<BeatshipHealthComponent>(out BeatshipHealthComponent health)
                         .AddComponent<BeatshipViewComponent>(out BeatshipViewComponent view)
                         .AddComponent<CollisionComponent>(out CollisionComponent collision)
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<RigidbodyComponent>(out RigidbodyComponent rigidbody)
                         .AddComponent<PidComponent>(out PidComponent pid)
                         .AddComponent<BeatshipLastVelocityComponent>()
                         .AddComponent<BeatshipTagComponent>()
                         .AddComponent<AntiGravityComponent>()
                         .AddComponent<PidValueComponent>()
                         .AddComponent<SpeedComponent>();

            maxCollisionAngle.maxCollisionAngle = 40.0f;

            transform.transform = beatshipInstance.transform;

            rigidbody.rigidbody = beatshipInstance.GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody.rigidbody, "Beatship rigidbody not found");

            var collisionBehaviour = beatshipInstance.GetComponent<CollisionBehaviour>();

            collision.collider = collisionBehaviour.collider;
            collision.collisionsOnEnter = collisionBehaviour.collisionsOnEnter;
            collision.collisionsOnExit = collisionBehaviour.collisionsOnExit;
            collision.collisionsOnStay = collisionBehaviour.collisionsOnStay;

            health.health = 100.0f;

            pid.kp = 10.0f;
            pid.ki = 0.0f;
            pid.kd = 1.0f;

            altitude.defaultAltitude = 4.0f;
            altitude.minAltitude = 2.0f;
            altitude.maxAltitude = 7.0f;

            PitchRotationBehaviour pitchRotationBehaviour =
                beatshipInstance.GetComponentInChildren<PitchRotationBehaviour>();
            Assert.IsNotNull(pitchRotationBehaviour, "Pitch rotation container not found. Beatship model must contained in game object which contains PitchRotationBehaviour script");

            RollRotationBehaviour rollRotationBehaviour =
                beatshipInstance.GetComponentInChildren<RollRotationBehaviour>();
            Assert.IsNotNull(pitchRotationBehaviour, "Roll rotation container not found. Beatship model must contained in game object which contains RollRotationBehaviour script");

            modelTransform.pitchTransform = pitchRotationBehaviour.transform;
            modelTransform.rollTransform = rollRotationBehaviour.transform;

            BeatshipBehaviour beatshipBehaviour = beatshipInstance.GetComponent<BeatshipBehaviour>();
            Assert.IsNotNull(beatshipBehaviour, "Beatship behaviour script on beatship prefab not found");

            view.viewOpacityCurve = beatshipBehaviour.viewOpacityCurve;
            view.viewRadius = beatshipBehaviour.viewRadius;

            rotation.pitchCurve = beatshipBehaviour.pitchCurve;
            rotation.rollLimit = beatshipBehaviour.rollLimit;
            rotation.pitchLimit = beatshipBehaviour.pitchLimit;
            rotation.rollRotationSpeed = beatshipBehaviour.rollRotationSpeed;
            rotation.pitchRotationSpeed = beatshipBehaviour.pitchRotationSpeed;
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