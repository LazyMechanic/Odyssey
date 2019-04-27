using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BarrierAreaSpawnEvent : IEcsAutoResetComponent
    {
        public Vector3 position;
        public Transform parent;
        public GameObject barrierAreaPrefab;

        public void Reset()
        {
            parent = null;
            barrierAreaPrefab = null;
        }
    }
}