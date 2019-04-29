using System.Collections.Generic;
using Leopotam.Ecs;

namespace Odyssey {
    sealed class BarrierAreaMapComponent : IEcsAutoResetComponent
    {
        public LinkedList<LinkedList<EcsEntity>> map;

        public void Reset()
        {
            foreach (var row in map)
            {
                row.Clear();
            }

            map.Clear();

            map = null;
        }
    }
}