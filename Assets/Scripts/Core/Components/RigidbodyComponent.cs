using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class RigidbodyComponent : IEcsAutoResetComponent
    {
        public Rigidbody rigidbody;
        
        public void Reset()
        {
            rigidbody = null;
        }
    }
}