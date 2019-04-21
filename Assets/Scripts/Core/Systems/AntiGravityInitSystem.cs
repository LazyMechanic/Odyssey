using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class AntiGravityInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;
        
        void IEcsInitSystem.Initialize () {
            // Add your initialize code here.
        }

        void IEcsInitSystem.Destroy () { }
    }
}