using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class AntiGravityApplySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<RigidbodyComponent, AntiGravityComponent> _objectFilter;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _objectFilter)
            {
                _objectFilter.Components1[i].rigidbody.AddForce(_objectFilter.Components2[i].force, ForceMode.Force);
            }
        }
    }
}