using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class LevelClearSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewRadiusComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run () {
            var map = _barrierAreaMapFilter.Components1[0].map;
            List<LinkedList<EcsEntity>> rowsToDelete = new List<LinkedList<EcsEntity>>();

            foreach (var row in map)
            {
                // Find barrier areas out view radius
                var barrierAreasToDelete = row.Where(barrierAreaEntity =>
                {
                    TransformComponent barrierAreaTransformComponent =
                        _world.GetComponent<TransformComponent>(barrierAreaEntity);
                    SizeComponent barrierAreaSizeComponent = _world.GetComponent<SizeComponent>(barrierAreaEntity);

                    float viewDiameter = _beatshipFilter.Components3[0].viewRadius * 2;

                    Bounds barrierAreaBounds = new Bounds(barrierAreaTransformComponent.transform.position,
                                                          barrierAreaSizeComponent.size);
                    Bounds viewAreaBounds = new Bounds(_beatshipFilter.Components2[0].transform.position,
                                                       new Vector3(viewDiameter,
                                                                   viewDiameter,
                                                                   viewDiameter));

                    return !IsRectangleOverlapRectangle(barrierAreaBounds, viewAreaBounds);
                });

                // Destroy barrier areas out view radius
                foreach (var barrierAreaEntity in barrierAreasToDelete)
                {
                    EntityBuilder.Instance(_world)
                                 .CreateEntity()
                                 .AddComponent<BarrierAreaDestroyEvent>(out BarrierAreaDestroyEvent destroyEvent);

                    destroyEvent.barrierAreaEntity = barrierAreaEntity;
                }
            }
        }

        bool IsRectangleOverlapCircle(Vector3 circlePosition, float circleRadius, Vector3 rectanglePosition, Vector3 rectangleSize)
        {
            Vector3 nearPointPosition = new Vector3(
                x: Mathf.Min(rectanglePosition.x + rectangleSize.x / 2,
                             Mathf.Max(rectanglePosition.x - rectangleSize.x / 2, circlePosition.x)),
                y: 0,
                z: Mathf.Min(rectanglePosition.z + rectangleSize.z / 2,
                             Mathf.Max(rectanglePosition.z - rectangleSize.z / 2, circlePosition.z)));

            float distanceToPoint = Vector3.Distance(circlePosition, nearPointPosition);
            return distanceToPoint > circleRadius;
        }

        bool IsRectangleOverlapRectangle(Bounds left, Bounds right)
        {
            return left.Intersects(right);
        }
    }
}