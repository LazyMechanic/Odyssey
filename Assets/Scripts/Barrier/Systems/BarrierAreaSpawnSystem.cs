using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierAreaSpawnSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaSpawnEvent> _filter;

        private bool _test = false;

        void IEcsRunSystem.Run () {
            foreach (var i in _filter)
            {
                var go = GameObject.Instantiate(_filter.Components1[i].barrierAreaPrefab,
                                                _filter.Components1[i].parent);

                go.name = _filter.Components1[i].barrierAreaPrefab.name;
                go.transform.position = _filter.Components1[i].position;

                var collider = go.GetComponent<Collider>();
                Assert.IsNotNull(collider, "BarrierArea collider not found");

                EntityBuilder.Instance(_world)
                             .CreateEntity()
                             .AddComponent<BarrierAreaTagComponent>()
                             .AddComponent<TransformComponent>(out TransformComponent transform)
                             .AddComponent<SizeComponent>(out SizeComponent size);

                transform.transform = go.transform;
                size.size = collider.bounds.size;

                CreateRectPatternEvents(go);
                CreateLinePatternEvents(go);
            }
        }

        void CreateRectPatternEvents(GameObject barrierAreaInstance)
        {
            var rectPatterns = barrierAreaInstance.GetComponentsInChildren<BarrierRectPatternBehaviour>();

            foreach (var behaviour in rectPatterns)
            {
                EntityBuilder.Instance(_world)
                             .CreateEntity()
                             .AddComponent<BarrierRectPatternGenerateEvent>(out BarrierRectPatternGenerateEvent e);

                e.barrierPrefab = GetRandomBarrierPrefab();
                e.patternGameObject = behaviour.gameObject;
                e.size = behaviour.size;
                e.density = behaviour.density;
                e.fillType = behaviour.fillType;
            }
        }

        void CreateLinePatternEvents(GameObject barrierAreaInstance)
        {
            var linePatterns = barrierAreaInstance.GetComponentsInChildren<BarrierLinePatternBehaviour>();

            foreach (var behaviour in linePatterns)
            {
                EntityBuilder.Instance(_world)
                             .CreateEntity()
                             .AddComponent<BarrierLinePatternGenerateEvent>(out BarrierLinePatternGenerateEvent e);

                e.barrierPrefab = GetRandomBarrierPrefab();
                e.patternGameObject = behaviour.gameObject;
                e.size = behaviour.size;
                e.density = behaviour.density;
                e.fillType = behaviour.fillType;
            }
        }

        GameObject GetRandomBarrierPrefab()
        {
            int num = Random.Range(1, 2);
            string path = "Barrier/Barrier_" + num;

            return Resources.Load<GameObject>(path);
        }
    }
}