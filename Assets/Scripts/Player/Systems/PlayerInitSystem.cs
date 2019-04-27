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