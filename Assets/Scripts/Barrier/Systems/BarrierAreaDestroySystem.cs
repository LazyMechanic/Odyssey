using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaDestroySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewRadiusComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run ()
        {
            var map = _barrierAreaMapFilter.Components1[0].map;
            var rowsToDelete = map.Where(row =>
            {
                EcsEntity entity = row.First.Value;

                TransformComponent barrierAreaTransformComponent = _world.GetComponent<TransformComponent>(entity);
                SizeComponent barrierAreaSizeComponent = _world.GetComponent<SizeComponent>(entity);

                Bounds barrierAreaBounds = new Bounds(barrierAreaTransformComponent.transform.position, barrierAreaSizeComponent.size);

                float bottomBorderZ = _beatshipFilter.Components2[0].transform.position.z -
                                      _beatshipFilter.Components3[0].viewRadius;

                return barrierAreaBounds.max.z < bottomBorderZ;
            }).ToList();

            foreach (var row in rowsToDelete)
            {
                foreach (var barrierAreaEntity in row)
                {
                    Transform transform = _world.GetComponent<TransformComponent>(barrierAreaEntity).transform;
                    GameObject.Destroy(transform.gameObject);

                    _world.RemoveEntity(barrierAreaEntity);
                }

                map.Remove(row);
            }
        }
    }
}