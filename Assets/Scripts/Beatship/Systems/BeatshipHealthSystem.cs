using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipHealthSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;
        
        void IEcsRunSystem.Run () {
            // Add your run code here.
        }
    }
}