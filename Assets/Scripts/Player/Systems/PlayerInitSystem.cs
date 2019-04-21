using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class PlayerInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PlayerTagComponent> _playerFilter;
        
        void IEcsInitSystem.Initialize ()
        {
            var playerGameObject = GameObject.FindObjectOfType<PlayerBehaviour>();
            Assert.IsNotNull(playerGameObject, "Player game object not found. It must contain PlayerBehaviour script");

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<PlayerTagComponent>()
                         .AddComponent<TransformComponent>(out TransformComponent transform);

            transform.transform = playerGameObject.transform;
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _playerFilter)
            {
                _world.RemoveEntity(_playerFilter.Entities[i]);
            }
        }
    }
}