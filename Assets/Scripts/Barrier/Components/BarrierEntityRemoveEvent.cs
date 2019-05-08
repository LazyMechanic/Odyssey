using Leopotam.Ecs;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierEntityRemoveEvent
    {
        public EcsEntity barrierEntity;
    }
}