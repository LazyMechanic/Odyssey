using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BarrierOpacitySystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BarrierAreaTagComponent, BarrierListComponent> _barrierAreaFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, BeatshipViewComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run () {
            foreach (var i in _barrierAreaFilter)
            {
                foreach (var barrierBehaviour in _barrierAreaFilter.Components2[i].barriers)
                {
                    float beatshipAndBarrierDistance = Vector3.Distance(
                        _beatshipFilter.Components2[0].transform.position, barrierBehaviour.transform.position);

                    float viewRadius = _beatshipFilter.Components3[0].viewRadius;
                    float opacity = _beatshipFilter
                                    .Components3[0]
                                    .viewOpacityCurve
                                    .Evaluate(Mathf.Max(1.0f - beatshipAndBarrierDistance / viewRadius, 0.0f));

                    barrierBehaviour.materialPropertyBlock.SetFloat("opacity", opacity);
                    barrierBehaviour.renderer.SetPropertyBlock(barrierBehaviour.materialPropertyBlock);
                }
            }
        }
    }
}