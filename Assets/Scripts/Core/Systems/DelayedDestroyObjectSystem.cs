using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class DelayedDestroyObjectSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<DelayDestroyObjectEvent> _destroyEventFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in _destroyEventFilter)
            {
                GameObject.Destroy(_destroyEventFilter.Components1[i].gameObject);
            }
        }
    }
}