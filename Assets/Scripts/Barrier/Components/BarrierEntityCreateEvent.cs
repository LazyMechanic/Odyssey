using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierEntityCreateEvent
    {
        public EcsEntity parentBarrierAreaEntity;
    }
}