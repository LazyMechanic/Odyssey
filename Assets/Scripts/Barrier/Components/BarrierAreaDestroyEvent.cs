using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierAreaDestroyEvent
    {
        public EcsEntity barrierAreaEntity;
    }
}