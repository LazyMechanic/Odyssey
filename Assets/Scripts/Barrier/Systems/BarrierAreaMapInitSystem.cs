using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaMapInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _mapFilter = null;
        private EcsFilter<BarrierAreaTagComponent> _barrierAreaFilter = null;

        void IEcsInitSystem.Initialize ()
        {
            CreateMapEntity();
        }

        void CreateMapEntity()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaMapComponent>(out BarrierAreaMapComponent map);

            map.map = new LinkedList<LinkedList<EcsEntity>>();
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _mapFilter)
            {
                _world.RemoveEntity(_mapFilter.Entities[i]);
            }

            foreach (var i in _barrierAreaFilter)
            {
                _world.RemoveEntity(_barrierAreaFilter.Entities[i]);
            }
        }
    }
}