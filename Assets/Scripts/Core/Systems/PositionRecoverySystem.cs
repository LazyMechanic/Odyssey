using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class PositionRecoverySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PositionRecoveryEvent, DeltaPositionComponent, TransformComponent> _filter;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _filter)
            {
                _filter.Components3[i].transform.position += _filter.Components2[i].deltaPosition;
            }
        }
    }
}