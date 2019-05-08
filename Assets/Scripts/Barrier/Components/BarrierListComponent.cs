using System.Collections.Generic;
using Leopotam.Ecs;

namespace Odyssey {
    sealed class BarrierListComponent : IEcsAutoResetComponent
    {
        public List<BarrierBehaviour> barriers;

        public void Reset()
        {
            barriers.Clear();
            barriers = null;
        }
    }
}