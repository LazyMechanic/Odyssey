using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierEntityCreateEvent : IEcsAutoResetComponent
    {
        public Transform transform;
        public MaterialPropertyBlock materialPropertyBlock;

        public void Reset()
        {
            transform = null;
            materialPropertyBlock = null;
        }
    }
}