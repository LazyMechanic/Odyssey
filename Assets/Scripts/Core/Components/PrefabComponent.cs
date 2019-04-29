using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey
{
    sealed class PrefabComponent : IEcsAutoResetComponent
    {
        public GameObject prefab;

        public void Reset()
        {
            prefab = null;
        }
    }
}