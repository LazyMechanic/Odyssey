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
        private EcsFilter<BarrierAreaContainerTagComponent> _containerFilter = null;

        void IEcsInitSystem.Initialize ()
        {
            CreateContainerEntity();
            CreateMapEntity();
        }

        void CreateContainerEntity()
        {
            var prefab = GetContainerPrefab();
            var go = GameObject.Instantiate(prefab);

            go.name = prefab.name;

            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaContainerTagComponent>()
                         .AddComponent<TransformComponent>(out TransformComponent transform);

            transform.transform = go.transform;
        }

        void CreateMapEntity()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaMapComponent>(out BarrierAreaMapComponent map);

            map.map = new LinkedList<LinkedList<EcsEntity>>();
        }

        GameObject GetContainerPrefab()
        {
            string path = "Barrier/BarrierAreaContainer";
            return Resources.Load<GameObject>(path);
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

            foreach (var i in _containerFilter)
            {
                _world.RemoveEntity(_containerFilter.Entities[i]);
            }
        }
    }
}