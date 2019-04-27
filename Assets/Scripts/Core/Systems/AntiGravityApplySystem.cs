using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class AntiGravityApplySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<RigidbodyComponent, AntiGravityComponent> _filter;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _filter)
            {
                _filter.Components1[i].rigidbody.AddForce(_filter.Components2[i].force, ForceMode.Force);
            }
        }
    }
}