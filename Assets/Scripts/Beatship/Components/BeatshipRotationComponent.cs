using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class BeatshipRotationComponent : IEcsAutoResetComponent
    {
        public AnimationCurve pitchCurve;
        public float rollLimit;
        public float pitchLimit;
        public float rollRotationSpeed;
        public float pitchRotationSpeed;

        public void Reset()
        {
            pitchCurve = null;
        }
    }
}