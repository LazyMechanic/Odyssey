using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaForwardSpawnSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaContainerTagComponent, TransformComponent> _barrierAreaContainerFilter = null;
        private EcsFilter<BarrierAreaTagComponent, TransformComponent, SizeComponent> _barrierAreaFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewRadiusComponent> _beatshipFilter = null;
        
        void IEcsRunSystem.Run ()
        {
            Vector3 beatshipPosition = _beatshipFilter.Components2[0].transform.position;
            float viewRadius = _beatshipFilter.Components3[0].viewRadius;

            Bounds farBarrierAreaBounds = new Bounds(beatshipPosition, Vector3.zero);
            bool needSpawn = true;
            foreach (var i in _barrierAreaFilter)
            {
                Bounds barrierAreaBounds = new Bounds(_barrierAreaFilter.Components2[i].transform.position,
                                                      _barrierAreaFilter.Components3[i].size);

                if (barrierAreaBounds.max.z > farBarrierAreaBounds.max.z)
                    farBarrierAreaBounds = barrierAreaBounds;

                // If at least one area is farther than view radius then not need spawn new area
                if (barrierAreaBounds.max.z >= beatshipPosition.z + viewRadius)
                {
                    needSpawn = false;
                    break;
                }
            }

            // Generate spawn event
            if (needSpawn)
            {
                EntityBuilder.Instance(_world)
                             .CreateEntity()
                             .AddComponent<BarrierAreaSpawnEvent>(out BarrierAreaSpawnEvent spawnEvent);

                spawnEvent.parent = _barrierAreaContainerFilter.Components2[0].transform;
                spawnEvent.barrierAreaPrefab = GetRandomBarrierAreaPrefab();

                var barrierAreaCollider = spawnEvent.barrierAreaPrefab.GetComponent<BoxCollider>();
                Assert.IsNotNull(barrierAreaCollider, "BarrierArea collider not found");

                spawnEvent.position = new Vector3(beatshipPosition.x, 0, farBarrierAreaBounds.max.z + barrierAreaCollider.size.z / 2);
            }
        }

        GameObject GetRandomBarrierAreaPrefab()
        {
            int num = Random.Range(1, 3);
            string path = "Barrier/BarrierArea_" + num;

            return Resources.Load<GameObject>(path);
        }
    }
}