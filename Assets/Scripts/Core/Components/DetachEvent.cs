using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class DetachEvent : IEcsAutoResetComponent
    {
        public Transform toDetach;
        public Vector3 position;

        public void Reset()
        {
            toDetach = null;
        }
    }
}