using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class RendererComponent : IEcsAutoResetComponent
    {
        public Renderer renderer;

        public void Reset()
        {
            renderer = null;
        }
    }
}