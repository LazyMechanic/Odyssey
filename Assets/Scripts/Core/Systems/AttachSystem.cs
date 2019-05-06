using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class AttachSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<AttachEvent> _attachEventFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in _attachEventFilter)
            {
                _attachEventFilter.Components1[i].toAttach.parent = _attachEventFilter.Components1[i].parent;
                _attachEventFilter.Components1[i].toAttach.localPosition = _attachEventFilter.Components1[i].localPosition;
            }
        }
    }
}