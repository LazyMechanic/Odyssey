using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipPositionRecoverySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipTagComponent, TransformComponent> _beatshipFilter = null;
        private EcsFilter<BarrierAreaTagComponent, TransformComponent> _barrierAreaFilter = null;
        private EcsFilter<BeatshipSpawnerTagComponent, TransformComponent, BeatshipFreeFlyRadiusComponent> _beatshipSpawnerFilter = null;

        void IEcsRunSystem.Run () {
            Vector3 deltaPosition = _beatshipSpawnerFilter.Components2[0].transform.position -
                                    _beatshipFilter.Components2[0].transform.position;

            deltaPosition.y = 0.0f;

            float deltaPositionMagnitude = deltaPosition.magnitude;

            if (deltaPositionMagnitude > _beatshipSpawnerFilter.Components3[0].freeFlyRadius)
            {
                // Move beatship
                MoveGameObject(_beatshipFilter.Components2[0].transform, deltaPosition);

                // Move barrier areas
                foreach (var i in _barrierAreaFilter)
                {
                    MoveGameObject(_barrierAreaFilter.Components2[i].transform, deltaPosition);
                }
            }
        }

        void MoveGameObject(Transform transform, Vector3 deltaPosition)
        {
            transform.position += deltaPosition;
        }
    }
}