using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierEntityCreateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierEntityCreateEvent> _barrierCreateEventFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            foreach (var i in _barrierCreateEventFilter)
            {
                EcsEntity barrierAreaEntity = _barrierCreateEventFilter.Components1[i].parentBarrierAreaEntity;
                var targetBarrierList =
                    _world.GetComponent<BarrierListComponent>(barrierAreaEntity).barriers;

                var barrierBehaviours = _world.GetComponent<TransformComponent>(barrierAreaEntity)
                                              .transform
                                              .gameObject
                                              .GetComponentsInChildren<BarrierBehaviour>();

                foreach (var behaviour in barrierBehaviours)
                {
                    targetBarrierList.Add(behaviour);
                }
            }
        }
    }
}