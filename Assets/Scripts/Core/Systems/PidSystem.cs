using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class PidSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<PidComponent, PidValueComponent> _pidFilter;

        void IEcsRunSystem.Run()
        {
            foreach (var i in _pidFilter)
            {
                ComputePid(i);
            }
        }

        private void ComputePid(int index)
        {
            float pidError = ComputePidError(index);
            float derivative = (pidError - _pidFilter.Components1[index].lastError) / Time.fixedDeltaTime;
            _pidFilter.Components1[index].integral += pidError * Time.fixedDeltaTime;
            _pidFilter.Components1[index].lastError = pidError;

            _pidFilter.Components1[index].value =
                Mathf.Clamp01(_pidFilter.Components1[index].kp * pidError +
                              _pidFilter.Components1[index].ki * _pidFilter.Components1[index].integral +
                              _pidFilter.Components1[index].kd * derivative);
        }

        private float ComputePidError(int index)
        {
            return _pidFilter.Components2[index].target - _pidFilter.Components2[index].current;
        }
    }
}