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
            var playerBehaviour = GameObject.FindObjectOfType<PlayerBehaviour>();
            Assert.IsNotNull(playerBehaviour, "Player not found. Object must contains PlayerBehavior component");

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<TransformComponent>(out TransformComponent transformComponent)
                         .AddComponent<PlayerTagComponent>();

            transformComponent.transform = playerBehaviour.transform;
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