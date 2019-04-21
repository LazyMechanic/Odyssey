using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipSpawnInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipSpawnTagComponent> _beatshipSpawnFilter = null;

        void IEcsInitSystem.Initialize ()
        {
            var spawnGameObject = GameObject.FindObjectOfType<BeatshipSpawnBehaviour>();
            Assert.IsNotNull(spawnGameObject, "Beatship spawn game object not found. It must contain BeatshipSpawnBehaviour script");

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BeatshipSpawnTagComponent>()
                         .AddComponent<TransformComponent>(out TransformComponent transform);

            transform.transform = spawnGameObject.transform;
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _beatshipSpawnFilter)
            {
                _world.RemoveEntity(_beatshipSpawnFilter.Entities[i]);
            }
        }
    }
}