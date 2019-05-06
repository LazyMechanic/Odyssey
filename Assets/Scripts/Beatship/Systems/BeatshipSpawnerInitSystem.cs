using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipSpawnerInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipSpawnerTagComponent> _beatshipSpawnerFilter = null;

        void IEcsInitSystem.Initialize ()
        {
            var spawnerInstance = GameObject.FindObjectOfType<BeatshipSpawnerBehaviour>();
            Assert.IsNotNull(spawnerInstance, "Beatship spawner game object not found. It must contain BeatshipSpawnerBehaviour script");

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BeatshipSpawnerTagComponent>()
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<BeatshipFreeFlyRadiusComponent>(out BeatshipFreeFlyRadiusComponent freeFlyRadius);

            transform.transform = spawnerInstance.transform;
            freeFlyRadius.freeFlyRadius = spawnerInstance.freeFlyRadius;
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _beatshipSpawnerFilter)
            {
                _world.RemoveEntity(_beatshipSpawnerFilter.Entities[i]);
            }
        }
    }
}