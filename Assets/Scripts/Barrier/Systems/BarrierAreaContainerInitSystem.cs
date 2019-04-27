using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaContainerInitSystem : IEcsInitSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaContainerTagComponent> _filter;
        
        void IEcsInitSystem.Initialize ()
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

        GameObject GetContainerPrefab()
        {
            string path = "Barrier/BarrierAreaContainer";
            return Resources.Load<GameObject>(path);
        }

        void IEcsInitSystem.Destroy()
        {
            foreach (var i in _filter)
            {
                _world.RemoveEntity(_filter.Entities[i]);
            }
        }
    }
}