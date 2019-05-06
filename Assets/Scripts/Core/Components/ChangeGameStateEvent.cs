using Leopotam.Ecs;

namespace Odyssey {
    [EcsOneFrame]
    sealed class ChangeGameStateEvent
    {
        public GameConfig.GameState gameState;
    }
}