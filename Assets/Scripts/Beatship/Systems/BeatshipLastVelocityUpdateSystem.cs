using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipLastVelocityUpdateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipTagComponent, RigidbodyComponent, BeatshipLastVelocityComponent> _beatshipFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            _beatshipFilter.Components3[0].lastVelocity = _beatshipFilter.Components2[0].rigidbody.velocity;
        }
    }
}