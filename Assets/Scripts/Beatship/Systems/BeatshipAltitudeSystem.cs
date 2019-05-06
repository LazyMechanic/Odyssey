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
            if (!_beatshipFilter.IsEmpty())
            {
                float currentAltitude = GetDistance(_beatshipFilter.Components3[0].transform.position,
                                                    -_beatshipFilter.Components3[0].transform.up);

                float inputAxis = _axisFilter.Components1[0].vertical;
                float defaultAltitude = _beatshipFilter.Components4[0].defaultAltitude;
                float maxAltitude = _beatshipFilter.Components4[0].maxAltitude;
                float minAltitude = _beatshipFilter.Components4[0].minAltitude;

                float targetAltitude = defaultAltitude;
                if (inputAxis >= 0.0f)
                {
                    targetAltitude = defaultAltitude + (maxAltitude - defaultAltitude) * inputAxis;
                }
                else
                {
                    targetAltitude = defaultAltitude - (minAltitude - defaultAltitude) * inputAxis;
                }

                _beatshipFilter.Components2[0].current = currentAltitude;
                _beatshipFilter.Components2[0].target = targetAltitude;
            }
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