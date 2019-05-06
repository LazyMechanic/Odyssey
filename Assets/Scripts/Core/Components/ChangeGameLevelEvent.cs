using Leopotam.Ecs;

namespace Odyssey {
    [EcsOneFrame]
    sealed class ChangeGameLevelEvent
    {
        public GameConfig.GameLevel gameLevel;
    }
}