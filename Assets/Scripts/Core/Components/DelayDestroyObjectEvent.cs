using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class DelayDestroyObjectEvent : IEcsAutoResetComponent
    {
        public GameObject gameObject;

        public void Reset()
        {
            gameObject = null;
        }
    }
}