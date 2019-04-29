using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaSpawnSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;
        private EcsFilter<BarrierAreaContainerTagComponent, TransformComponent> _barrierAreaContainerFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewRadiusComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run ()
        {
            SpawnRows();
            SpawnColumns();
        }

        void SpawnRows()
        {
            Vector3 beatshipPosition = _beatshipFilter.Components2[0].transform.position;
            float viewRadius = _beatshipFilter.Components3[0].viewRadius;

            Bounds farBarrierAreaBounds = new Bounds(beatshipPosition, Vector3.zero);
            bool needSpawn = true;

            var map = _barrierAreaMapFilter.Components1[0].map;

            if (map.Count > 0)
            {
                EcsEntity barrierAreaEntity = map.First.Value.First.Value;

                TransformComponent barrierAreaTransformComponent = _world.GetComponent<TransformComponent>(barrierAreaEntity);
                SizeComponent barrierAreaSizeComponent = _world.GetComponent<SizeComponent>(barrierAreaEntity);

                Bounds barrierAreaBounds = new Bounds(barrierAreaTransformComponent.transform.position,
                                                      barrierAreaSizeComponent.size);

                if (barrierAreaBounds.max.z > farBarrierAreaBounds.max.z)
                    farBarrierAreaBounds = barrierAreaBounds;

                // If at least one area is farther than view radius then not need spawn new area
                if (barrierAreaBounds.max.z >= beatshipPosition.z + viewRadius)
                {
                    needSpawn = false;
                }
            }

            // Generate spawn event
            if (needSpawn)
            {
                GameObject barrierAreaPrefab = GetBarrierAreaPrefab();

                BoxCollider barrierAreaCollider = barrierAreaPrefab.GetComponent<BoxCollider>();
                Assert.IsNotNull(barrierAreaCollider, "BarrierArea collider not found");

                Vector3 position = new Vector3(beatshipPosition.x, 0, farBarrierAreaBounds.max.z + barrierAreaCollider.size.z / 2);

                Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                // Spawn barrier area
                EcsEntity barrierAreaEntity = SpawnBarrierArea(barrierAreaPrefab, position, parent);

                // Create new row
                LinkedList<EcsEntity> newRow = new LinkedList<EcsEntity>();
                newRow.AddFirst(barrierAreaEntity);

                // Add new row on map
                map.AddFirst(newRow);
            }
        }

        void SpawnColumns()
        {
            SpawnOnLeftSide();
            SpawnOnRightSize();
        }

        void SpawnOnLeftSide()
        {
            var map = _barrierAreaMapFilter.Components1[0].map;

            // map.First is left barrier area
            foreach (var row in map)
            {
                EcsEntity leftEntity = row.First.Value;

                TransformComponent leftBarrierAreaTransformComponent = _world.GetComponent<TransformComponent>(leftEntity);
                SizeComponent leftBarrierAreaSizeComponent = _world.GetComponent<SizeComponent>(leftEntity);

                Bounds leftBarrierAreaBounds = new Bounds(leftBarrierAreaTransformComponent.transform.position, leftBarrierAreaSizeComponent.size);

                float leftBorderX = _beatshipFilter.Components2[0].transform.position.x -
                                    _beatshipFilter.Components3[0].viewRadius;

                // If the leftmost area is to the right of the left border
                if (leftBarrierAreaBounds.min.x > leftBorderX)
                {
                    Vector3 position = new Vector3(leftBarrierAreaBounds.min.x - leftBarrierAreaBounds.size.x / 2,
                                                   leftBarrierAreaTransformComponent.transform.position.y,
                                                   leftBarrierAreaTransformComponent.transform.position.z);

                    PrefabComponent prefabComponent = _world.GetComponent<PrefabComponent>(leftEntity);

                    Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                    // Create barrier area
                    EcsEntity entity = SpawnBarrierArea(prefabComponent.prefab, position, parent);

                    // Add area to row
                    row.AddFirst(entity);
                }
            }
        }

        void SpawnOnRightSize()
        {
            var map = _barrierAreaMapFilter.Components1[0].map;

            // map.First is left barrier area
            foreach (var row in map)
            {
                EcsEntity rightEntity = row.Last.Value;

                TransformComponent rightBarrierAreaTransformComponent = _world.GetComponent<TransformComponent>(rightEntity);
                SizeComponent rightBarrierAreaSizeComponent = _world.GetComponent<SizeComponent>(rightEntity);

                Bounds rightBarrierAreaBounds = new Bounds(rightBarrierAreaTransformComponent.transform.position, rightBarrierAreaSizeComponent.size);

                float rightBorderX = _beatshipFilter.Components2[0].transform.position.x +
                                     _beatshipFilter.Components3[0].viewRadius;

                // If the rightmost area is to the left of the right border
                if (rightBarrierAreaBounds.max.x < rightBorderX)
                {
                    Vector3 position = new Vector3(rightBarrierAreaBounds.max.x + rightBarrierAreaBounds.size.x / 2,
                                                   rightBarrierAreaTransformComponent.transform.position.y,
                                                   rightBarrierAreaTransformComponent.transform.position.z);

                    PrefabComponent prefabComponent = _world.GetComponent<PrefabComponent>(rightEntity);

                    Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                    // Create barrier area
                    EcsEntity entity = SpawnBarrierArea(prefabComponent.prefab, position, parent);

                    // Add area to row
                    row.AddLast(entity);
                }
            }
        }

        EcsEntity SpawnBarrierArea(GameObject barrierAreaPrefab, Vector3 position, Transform parent)
        {
            GameObject barrierAreaInstance = InstantiateBarrierArea(barrierAreaPrefab, position, parent);
            EcsEntity barrierAreaEntity = CreateBarrierAreaEntity(barrierAreaInstance, barrierAreaPrefab);
            GenerateRectPatterns(barrierAreaInstance);
            GenerateLinePatterns(barrierAreaInstance);

            return barrierAreaEntity;
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
                         .AddComponent<BarrierAreaTagComponent>()
                         .AddComponent<TransformComponent>(out TransformComponent transform)
                         .AddComponent<SizeComponent>(out SizeComponent size)
                         .AddComponent<PrefabComponent>(out PrefabComponent prefab);

            transform.transform = barrierAreaInstance.transform;

            var collider = barrierAreaInstance.GetComponent<Collider>();
            Assert.IsNotNull(collider, "BarrierArea collider not found");

            size.size = collider.bounds.size;

            prefab.prefab = barrierAreaPrefab;

            return entity;
        }

        void GenerateRectPatterns(GameObject barrierAreaInstance)
        {
            var rectPatterns = barrierAreaInstance.GetComponentsInChildren<BarrierRectPatternBehaviour>();

            foreach (var behaviour in rectPatterns)
            {
                CreateRectPatternEvent(behaviour);
            }
        }

        void CreateRectPatternEvent(BarrierRectPatternBehaviour barrierRectPattern)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierRectPatternGenerateEvent>(out BarrierRectPatternGenerateEvent generateEvent);

            generateEvent.barrierPrefab = GetBarrierPrefab();
            generateEvent.patternGameObject = barrierRectPattern.gameObject;
            generateEvent.size = barrierRectPattern.size;
            generateEvent.density = barrierRectPattern.density;
            generateEvent.fillType = barrierRectPattern.fillType;
        }

        void GenerateLinePatterns(GameObject barrierAreaInstance)
        {
            var linePatterns = barrierAreaInstance.GetComponentsInChildren<BarrierLinePatternBehaviour>();

            foreach (var behaviour in linePatterns)
            {
                CreateLinePatternEvent(behaviour);
            }
        }

        void CreateLinePatternEvent(BarrierLinePatternBehaviour barrierLinePattern)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierLinePatternGenerateEvent>(out BarrierLinePatternGenerateEvent generateEvent);

            generateEvent.barrierPrefab = GetBarrierPrefab();
            generateEvent.patternGameObject = barrierLinePattern.gameObject;
            generateEvent.size = barrierLinePattern.size;
            generateEvent.density = barrierLinePattern.density;
            generateEvent.fillType = barrierLinePattern.fillType;
        }

        GameObject GetBarrierPrefab()
        {
            int num = Random.Range(1, 2);
            string path = "Barrier/Barrier_" + num;

            return Resources.Load<GameObject>(path);
        }

        GameObject GetBarrierAreaPrefab()
        {
            int num = Random.Range(1, 3);
            string path = "Barrier/BarrierArea_" + num;

            return Resources.Load<GameObject>(path);
        }
    }
}