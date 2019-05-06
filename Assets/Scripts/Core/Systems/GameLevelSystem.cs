using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class GameLevelSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private GameConfig _gameConfig = null;
        private EcsFilter<ChangeGameLevelEvent> _changeGameLevelEventFilter = null;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _changeGameLevelEventFilter)
            {
                _gameConfig.gameLevel = _changeGameLevelEventFilter.Components1[i].gameLevel;
            }
        }
    }
}