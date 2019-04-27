using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class PositionRecoveryEvent : IEcsAutoResetComponent
    {
        public Transform transform;
        public Vector3 deltaPosition;

        public void Reset()
        {
            transform = null;
        }
    }
}