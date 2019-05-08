using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierLinePatternGenerateEvent : IEcsAutoResetComponent
    {
        public float size;
        public float density;
        public BarrierPatternFillType fillType;
        public GameObject barrierPrefab;
        public Transform parent;

        public void Reset()
        {
            barrierPrefab = null;
            parent = null;
        }
    }
}