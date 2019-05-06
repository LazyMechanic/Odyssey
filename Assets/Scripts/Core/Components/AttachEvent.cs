using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class AttachEvent : IEcsAutoResetComponent
    {
        public Transform toAttach;
        public Transform parent;
        public Vector3 localPosition;

        public void Reset()
        {
            toAttach = null;
            parent = null;
        }
    }
}