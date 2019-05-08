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
        private EcsFilter<BarrierAreaSpawnEvent> _barrierAreaSpawnEventFilter = null;
        private EcsFilter<BarrierAreaDestroyEvent> _barrierAreaDestroyEventFilter = null;
        private EcsFilter<BeatshipSpawnerTagComponent, TransformComponent> _beatshipSpawnerFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, RigidbodyComponent> _beatshipFilter = null;
        private EcsFilter<BarrierAreaContainerTagComponent, TransformComponent> _barrierAreaContainerFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in _mapResetEventFilter)
            {
                MoveBeatshipToSpawn();
                DeleteAllBarrierAreaSpawnEvents();
                DeleteAllBarrierAreaDestroyEvents();
                ClearBarrierAreaMap();
                CreateBarrierAreaBuffer();
            }
        }

        void DeleteAllBarrierAreaSpawnEvents()
        {
            foreach (var i in _barrierAreaSpawnEventFilter)
            {
                _world.RemoveEntity(_barrierAreaSpawnEventFilter.Entities[0]);
            }
        }

        void DeleteAllBarrierAreaDestroyEvents()
        {
            foreach (var i in _barrierAreaDestroyEventFilter)
            {
                _world.RemoveEntity(_barrierAreaDestroyEventFilter.Entities[0]);
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

        void MoveBeatshipToSpawn()
        {
            _beatshipFilter.Components2[0].transform.position =
                _beatshipSpawnerFilter.Components2[0].transform.position;

            _beatshipFilter.Components3[0].rigidbody.velocity = Vector3.zero;
        }
    }
}