using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaSpawnSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaSpawnEvent> _spawnEventFilter = null;
        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in _spawnEventFilter)
            {
                CreateBarrierArea(_spawnEventFilter.Components1[i]);
            }
        }

        void CreateBarrierArea(BarrierAreaSpawnEvent spawnEvent)
        {
            GameObject barrierAreaPrefab = spawnEvent.barrierAreaPrefab;
            Transform parent = spawnEvent.parent;
            Vector3 position = spawnEvent.position;
            LinkedList<EcsEntity> row = spawnEvent.row;
            BarrierAreaSpawnEvent.InsertPosition insertPosition = spawnEvent.insertPositionInRow;

            GameObject barrierAreaInstance = InstantiateBarrierArea(barrierAreaPrefab, position, parent);
            EcsEntity barrierAreaEntity = CreateBarrierAreaEntity(barrierAreaInstance, barrierAreaPrefab);
            CreateBarrierEntities(barrierAreaEntity);
            GeneratePatterns(barrierAreaInstance);
            AddBarrierAreaToRow(barrierAreaEntity, row, insertPosition);
        }

        GameObject InstantiateBarrierArea(GameObject barrierAreaPrefab, Vector3 position, Transform parent)
        {
            var go = GameObject.Instantiate(barrierAreaPrefab,
                                            parent);

            go.name = barrierAreaPrefab.name;
            go.transform.position = position;

            return go;
        }

        EcsEntity CreateBarrierAreaEntity(GameObject barrierAreaInstance, GameObject barrierAreaPrefab)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity(out EcsEntity entity)
                         .AddComponent<BarrierListComponent>(out BarrierListComponent barrierList)
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<PrefabComponent>(out PrefabComponent prefab)
                         .AddComponent<SizeComponent>(out SizeComponent size)
                         .AddComponent<BarrierAreaTagComponent>();

            barrierList.barriers = new List<BarrierBehaviour>();

            transform.transform = barrierAreaInstance.transform;

            var collider = barrierAreaInstance.GetComponent<Collider>();
            Assert.IsNotNull(collider, "BarrierArea collider not found");

            size.size = collider.bounds.size;

            prefab.prefab = barrierAreaPrefab;

            return entity;
        }

        void CreateBarrierEntities(EcsEntity barrierAreaEntity)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierEntityCreateEvent>(out BarrierEntityCreateEvent createEvent);

            createEvent.parentBarrierAreaEntity = barrierAreaEntity;
        }

        void GeneratePatterns(GameObject barrierAreaInstance)
        {
            GenerateLinePatterns(barrierAreaInstance);
            GenerateRectPatterns(barrierAreaInstance);
        }

        void GenerateRectPatterns(GameObject barrierAreaInstance)
        {
            var rectPatterns = barrierAreaInstance.GetComponentsInChildren<BarrierRectPatternBehaviour>();

            foreach (var behaviour in rectPatterns)
            {
                CreateRectPatternEvent(behaviour);
            }
        }

        void CreateRectPatternEvent(BarrierRectPatternBehaviour barrierRectPatternBehaviour)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierRectPatternGenerateEvent>(out BarrierRectPatternGenerateEvent generateEvent);

            generateEvent.barrierPrefab = barrierRectPatternBehaviour.barrierPrefab;
            generateEvent.parent = barrierRectPatternBehaviour.transform;
            generateEvent.size = barrierRectPatternBehaviour.size;
            generateEvent.density = barrierRectPatternBehaviour.density;
            generateEvent.fillType = barrierRectPatternBehaviour.fillType;
        }

        void GenerateLinePatterns(GameObject barrierAreaInstance)
        {
            var linePatterns = barrierAreaInstance.GetComponentsInChildren<BarrierLinePatternBehaviour>();

            foreach (var behaviour in linePatterns)
            {
                CreateLinePatternEvent(behaviour);
            }
        }

        void CreateLinePatternEvent(BarrierLinePatternBehaviour barrierLinePatternBehaviour)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierLinePatternGenerateEvent>(out BarrierLinePatternGenerateEvent generateEvent);

            generateEvent.barrierPrefab = barrierLinePatternBehaviour.barrierPrefab;
            generateEvent.parent = barrierLinePatternBehaviour.transform;
            generateEvent.size = barrierLinePatternBehaviour.size;
            generateEvent.density = barrierLinePatternBehaviour.density;
            generateEvent.fillType = barrierLinePatternBehaviour.fillType;
        }

        void AddBarrierAreaToRow(EcsEntity barrierAreaEntity, LinkedList<EcsEntity> row, BarrierAreaSpawnEvent.InsertPosition insertPosition)
        {
            if (row == null)
            {
                row = new LinkedList<EcsEntity>();
                _barrierAreaMapFilter.Components1[0].map.AddFirst(row);
            }

            if (insertPosition == BarrierAreaSpawnEvent.InsertPosition.First)
            {
                row.AddFirst(barrierAreaEntity);
            }
            else if (insertPosition == BarrierAreaSpawnEvent.InsertPosition.Last)
            {
                row.AddLast(barrierAreaEntity);
            }
        }
    }
}