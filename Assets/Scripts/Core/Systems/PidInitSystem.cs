using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class PidInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PidComponent> _filter;
        
        void IEcsInitSystem.Initialize ()
        {
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _filter)
            {
                _world.RemoveEntity(_filter.Entities[i]);
            }
        }
    }
}