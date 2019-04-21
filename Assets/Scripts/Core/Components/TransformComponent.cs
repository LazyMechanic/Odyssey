using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey
{
    sealed class TransformComponent : IEcsAutoResetComponent
    {
        public Transform transform;

        public void Reset()
        {
            transform = null;
        }
    }
}