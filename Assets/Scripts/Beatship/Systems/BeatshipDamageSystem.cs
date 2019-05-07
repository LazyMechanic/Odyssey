using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipDamageSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipAddDamageEvent> _addDamageEventFilter = null;
        private EcsFilter<BeatshipTagComponent, BeatshipHealthComponent> _beatshipFilter = null;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _addDamageEventFilter)
            {
                _beatshipFilter.Components2[0].health -= _addDamageEventFilter.Components1[i].damage;
                if (_beatshipFilter.Components2[0].health <= 0.0f)
                {

                }
            }
        }
    }
}