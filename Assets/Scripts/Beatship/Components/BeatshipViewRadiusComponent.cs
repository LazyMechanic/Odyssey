using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class BeatshipViewComponent : IEcsAutoResetComponent
    {
        public AnimationCurve viewOpacityCurve;
        public float viewRadius;

        public void Reset()
        {
            viewOpacityCurve = null;
        }
    }
}