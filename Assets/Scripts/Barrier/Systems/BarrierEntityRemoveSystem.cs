using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierEntityRemoveSystem : IEcsRunSystem, IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierTagComponent> _barrierFilter = null;
        private EcsFilter<BarrierEntityRemoveEvent> _barrierRemoveEventFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in _barrierRemoveEventFilter)
            {
                _world.RemoveEntity(_barrierRemoveEventFilter.Components1[i].barrierEntity);
            }
        }

        public void Initialize()
        {
            // Not use
        }

        public void Destroy()
        {
            foreach (var i in _barrierFilter)
            {
                _world.RemoveEntity(_barrierFilter.Entities[i]);
            }
        }
    }
}