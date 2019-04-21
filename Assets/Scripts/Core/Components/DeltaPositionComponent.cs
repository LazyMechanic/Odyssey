using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class DeltaPositionComponent
    {
        public Vector3 deltaPosition;
    }
}