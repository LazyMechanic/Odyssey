using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class MaterialPropertyBlockComponent : IEcsAutoResetComponent
    {
        public MaterialPropertyBlock materialPropertyBlock;

        public void Reset()
        {
            materialPropertyBlock = null;
        }
    }
}