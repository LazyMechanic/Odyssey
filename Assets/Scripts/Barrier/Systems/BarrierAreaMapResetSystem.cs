using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaMapResetSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _mapFilter = null;
        private EcsFilter<BarrierAreaMapResetEvent> _mapResetEventFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent> _beatshipFilter = null;
        private EcsFilter<BarrierAreaContainerTagComponent, TransformComponent> _barrierAreaContainerFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in _mapResetEventFilter)
            {
                ClearBarrierAreaMap();
                CreateBarrierAreaBuffer();
            }
        }

        void ClearBarrierAreaMap()
        {
            var map = _mapFilter.Components1[0].map;
            foreach (var row in map)
            {
                foreach (var barrierAreaEntity in row)
                {
                    EntityBuilder.Instance(_world)
                                 .CreateEntity()
                                 .AddComponent<BarrierAreaDestroyEvent>(out BarrierAreaDestroyEvent destroyEvent);

                    destroyEvent.barrierAreaEntity = barrierAreaEntity;
                }
            }
        }

        void CreateBarrierAreaBuffer()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaSpawnEvent>(out BarrierAreaSpawnEvent spawnEvent);

            spawnEvent.barrierAreaPrefab = GetBarrierAreaBufferPrefab();
            spawnEvent.position = _beatshipFilter.Components2[0].transform.position;
            spawnEvent.row = null;
            spawnEvent.parent = _barrierAreaContainerFilter.Components2[0].transform;
            spawnEvent.insertPositionInRow = BarrierAreaSpawnEvent.InsertPosition.Last;
        }

        GameObject GetBarrierAreaBufferPrefab()
        {
            string path = "Barrier/BarrierAreaBuffer";
            return Resources.Load<GameObject>(path);
        }
    }
}