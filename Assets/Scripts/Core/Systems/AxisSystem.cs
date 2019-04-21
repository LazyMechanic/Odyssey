using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class AxisSystem : IEcsRunSystem {
        // Auto-injected fields.
        private EcsWorld _world = null;

        private EcsFilter<AxisComponent> _axisFilter = null;

        void IEcsRunSystem.Run()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float thrust = Input.GetAxis("Thrust");

            foreach (var i in _axisFilter)
            {
                _axisFilter.Components1[i].horizontal = horizontal;
                _axisFilter.Components1[i].vertical = vertical;
                _axisFilter.Components1[i].thrust = thrust;
            }
        }
    }
}