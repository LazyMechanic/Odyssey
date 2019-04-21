using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class AxisInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<AxisComponent> _filter = null;

        void IEcsInitSystem.Initialize()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<AxisComponent>();
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