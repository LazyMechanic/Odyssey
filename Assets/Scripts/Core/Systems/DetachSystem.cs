using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class DetachSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<DetachEvent> _detachEventFilter = null;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _detachEventFilter)
            {
                _detachEventFilter.Components1[i].toDetach.parent = null;
                _detachEventFilter.Components1[i].toDetach.position = _detachEventFilter.Components1[i].position;
            }
        }
    }
}