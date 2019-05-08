using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class LevelGenerateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;
        private EcsFilter<BarrierAreaContainerTagComponent, TransformComponent> _barrierAreaContainerFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run ()
        {
            SpawnRows();
            SpawnColumns();
        }

        void SpawnRows()
        {
            Transform beatshipTransform = _beatshipFilter.Components2[0].transform;
            float beatshipViewRadius = _beatshipFilter.Components3[0].viewRadius;

            Vector3 beatshipPosition = beatshipTransform.position;
            float viewRadius = beatshipViewRadius;

            Bounds farBarrierAreaBounds = new Bounds(beatshipPosition, Vector3.zero);
            bool needSpawn = true;

            var map = _barrierAreaMapFilter.Components1[0].map;

            if (map.Count > 0)
            {
                var firstRow = map.First.Value;
                if (firstRow.Count > 0)
                {
                    EcsEntity barrierAreaEntity = firstRow.First.Value;

                    TransformComponent barrierAreaTransformComponent =
                        _world.GetComponent<TransformComponent>(barrierAreaEntity);
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
            }

            // Generate spawn event
            if (needSpawn)
            {
                GameObject barrierAreaPrefab = GetBarrierAreaPrefab();

                SetPatternBarrierPrefabs(barrierAreaPrefab);

                BoxCollider barrierAreaCollider = barrierAreaPrefab.GetComponent<BoxCollider>();
                Assert.IsNotNull(barrierAreaCollider, "BarrierArea collider not found");

                Vector3 position = new Vector3(beatshipPosition.x, 0, farBarrierAreaBounds.max.z + barrierAreaCollider.size.z / 2);

                Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                // Create barrier area
                SpawnBarrierArea(barrierAreaPrefab, position, parent, null,
                                 BarrierAreaSpawnEvent.InsertPosition.Last);
            }
        }
        
        void SpawnColumns()
        {
            Transform beatshipTransform = _beatshipFilter.Components2[0].transform;
            float beatshipViewRadius = _beatshipFilter.Components3[0].viewRadius;

            SpawnOnLeftSide(beatshipTransform, beatshipViewRadius);
            SpawnOnRightSize(beatshipTransform, beatshipViewRadius);
        }

        void SpawnOnLeftSide(Transform beatshipTransform, float beatshipViewRadius)
        {
            var map = _barrierAreaMapFilter.Components1[0].map;

            // map.First is left barrier area
            foreach (var row in map)
            {
                EcsEntity leftEntity = row.First.Value;

                TransformComponent leftBarrierAreaTransformComponent = _world.GetComponent<TransformComponent>(leftEntity);
                SizeComponent leftBarrierAreaSizeComponent = _world.GetComponent<SizeComponent>(leftEntity);

                Bounds leftBarrierAreaBounds = new Bounds(leftBarrierAreaTransformComponent.transform.position, leftBarrierAreaSizeComponent.size);

                float leftBorderX = beatshipTransform.position.x -
                                    beatshipViewRadius;

                // If the leftmost area is to the right of the left border
                if (leftBarrierAreaBounds.min.x > leftBorderX)
                {
                    Vector3 position = new Vector3(leftBarrierAreaBounds.min.x - leftBarrierAreaBounds.size.x / 2,
                                                   leftBarrierAreaTransformComponent.transform.position.y,
                                                   leftBarrierAreaTransformComponent.transform.position.z);

                    PrefabComponent prefabComponent = _world.GetComponent<PrefabComponent>(leftEntity);

                    Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                    SetPatternBarrierPrefabs(prefabComponent.prefab);

                    // Create barrier area
                    SpawnBarrierArea(prefabComponent.prefab, position, parent, row,
                                     BarrierAreaSpawnEvent.InsertPosition.First);
                }
            }
        }

        void SpawnOnRightSize(Transform beatshipTransform, float beatshipViewRadius)
        {
            var map = _barrierAreaMapFilter.Components1[0].map;

            // map.First is left barrier area
            foreach (var row in map)
            {
                EcsEntity rightBarrierAreaEntity = row.Last.Value;

                TransformComponent rightBarrierAreaTransformComponent = _world.GetComponent<TransformComponent>(rightBarrierAreaEntity);
                SizeComponent rightBarrierAreaSizeComponent = _world.GetComponent<SizeComponent>(rightBarrierAreaEntity);

                Bounds rightBarrierAreaBounds = new Bounds(rightBarrierAreaTransformComponent.transform.position, rightBarrierAreaSizeComponent.size);

                float rightBorderX = beatshipTransform.position.x +
                                     beatshipViewRadius;

                // If the rightmost area is to the left of the right border
                if (rightBarrierAreaBounds.max.x < rightBorderX)
                {
                    Vector3 position = new Vector3(rightBarrierAreaBounds.max.x + rightBarrierAreaBounds.size.x / 2,
                                                   rightBarrierAreaTransformComponent.transform.position.y,
                                                   rightBarrierAreaTransformComponent.transform.position.z);

                    PrefabComponent prefabComponent = _world.GetComponent<PrefabComponent>(rightBarrierAreaEntity);

                    Transform parent = _barrierAreaContainerFilter.Components2[0].transform;

                    SetPatternBarrierPrefabs(prefabComponent.prefab);

                    // Create barrier area
                    SpawnBarrierArea(prefabComponent.prefab, position, parent, row,
                                     BarrierAreaSpawnEvent.InsertPosition.Last);
                }
            }
        }

        void SpawnBarrierArea(GameObject barrierAreaPrefab, 
                                   Vector3 position, 
                                   Transform parent, 
                                   LinkedList<EcsEntity> row,
                                   BarrierAreaSpawnEvent.InsertPosition insertPosition)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaSpawnEvent>(out BarrierAreaSpawnEvent spawnEvent);

            spawnEvent.position = position;
            spawnEvent.barrierAreaPrefab = barrierAreaPrefab;
            spawnEvent.parent = parent;
            spawnEvent.row = row;
            spawnEvent.insertPositionInRow = insertPosition;
        }

        void SetPatternBarrierPrefabs(GameObject barrierAreaPrefab)
        {
            var linePatterns = barrierAreaPrefab.GetComponentsInChildren<BarrierLinePatternBehaviour>();
            foreach (var pattern in linePatterns)
            {
                if (pattern.barrierPrefab == null)
                    pattern.barrierPrefab = GetBarrierPrefab();
            }

            var rectPatterns = barrierAreaPrefab.GetComponentsInChildren<BarrierRectPatternBehaviour>();
            foreach (var pattern in rectPatterns)
            {
                if (pattern.barrierPrefab == null)
                    pattern.barrierPrefab = GetBarrierPrefab();
            }
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