using System.Linq.Expressions;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipAltitudeSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<AxisComponent> _axisFilter = null;
        private EcsFilter<BeatshipTagComponent, PidValueComponent, TransformComponent, BeatshipAltitudeComponent> _beatshipFilter = null;
        void IEcsRunSystem.Run ()
        {
            float currentAltitude = GetDistance(_beatshipFilter.Components3[0].transform.position,
                                                -_beatshipFilter.Components3[0].transform.up);
            float targetAltitude = _beatshipFilter.Components4[0].defaultAltitude +
                                   _beatshipFilter.Components4[0].defaultAltitude * 0.5f *
                                   _axisFilter.Components1[0].vertical;

            _beatshipFilter.Components2[0].current = currentAltitude;
            _beatshipFilter.Components2[0].target = targetAltitude;
        }

        private float GetDistance(Vector3 position, Vector3 direction)
        {
            RaycastHit hit;
            Physics.Raycast(position,
                            direction,
                            out hit, 
                            Mathf.Infinity);

            return hit.distance;
        }
    }
}