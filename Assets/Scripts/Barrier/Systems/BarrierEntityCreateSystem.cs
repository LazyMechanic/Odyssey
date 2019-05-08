using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierEntityCreateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierEntityCreateEvent> _barrierCreateEventFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in _barrierCreateEventFilter)
            {
                EntityBuilder.Instance(_world)
                             .CreateEntity()
                             .AddComponent<BarrierTagComponent>()
                             .AddComponent<MaterialPropertyBlockComponent>(
                                 out MaterialPropertyBlockComponent matPropertyBlock)
                             .AddComponent<TransformComponent>(out TransformComponent transform);

                matPropertyBlock.materialPropertyBlock = _barrierCreateEventFilter.Components1[i].materialPropertyBlock;
                transform.transform = _barrierCreateEventFilter.Components1[i].transform;
            }
        }
    }
}