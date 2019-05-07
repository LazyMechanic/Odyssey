using Leopotam.Ecs;

namespace Odyssey {
    [EcsOneFrame]
    sealed class BeatshipAddDamageEvent
    {
        public float damage;
    }
}