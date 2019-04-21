using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class AntiGravitySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PidComponent, AntiGravityComponent, TransformComponent> _pidFilter = null;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _pidFilter)
            {
                _pidFilter.Components2[i].force = _pidFilter.Components1[i].value *
                                                  _pidFilter.Components2[i].maxVerticalSize *
                                                  _pidFilter.Components3[i].transform.up;
            }
        }
    }
}