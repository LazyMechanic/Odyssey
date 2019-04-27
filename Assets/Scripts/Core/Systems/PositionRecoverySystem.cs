using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class PositionRecoverySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PositionRecoveryEvent> _filter;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _filter)
            {
                _filter.Components1[i].transform.position += _filter.Components1[i].deltaPosition;
            }
        }
    }
}