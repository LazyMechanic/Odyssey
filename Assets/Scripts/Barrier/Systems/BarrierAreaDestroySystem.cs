using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaDestroySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaDestroyEvent> _destroyEventFilter = null;
        private EcsFilter<BarrierAreaMapComponent> _barrierAreaMapFilter = null;

        void IEcsRunSystem.Run ()
        {
            var map = _barrierAreaMapFilter.Components1[0].map;
            foreach (var i in _destroyEventFilter)
            {
                EcsEntity barrierAreaEntity = _destroyEventFilter.Components1[i].barrierAreaEntity;

                bool isRemoveRowSuccess = false;
                foreach (var row in map)
                {
                    if (row.Remove(barrierAreaEntity))
                    {
                        isRemoveRowSuccess = true;

                        // If row is empty
                        if (row.Count == 0)
                        {
                            // Remove row from map
                            map.Remove(row);
                        }

                        break;
                    }
                }

                Assert.IsTrue(isRemoveRowSuccess, "Barrier area entity not found in map");

                GameObject barrierAreaInstance =
                    _world.GetComponent<TransformComponent>(barrierAreaEntity).transform.gameObject;

                // Destroy game object instance
                GameObject.Destroy(barrierAreaInstance);

                // Remove entity
                _world.RemoveEntity(barrierAreaEntity);
            }
        }
    }
}