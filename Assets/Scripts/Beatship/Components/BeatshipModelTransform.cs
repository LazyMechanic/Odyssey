using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class BeatshipModelTransform : IEcsAutoResetComponent
    {
        public Transform pitchTransform;
        public Transform rollTransform;

        public void Reset()
        {
            pitchTransform = null;
            rollTransform = null;
        }
    }
}