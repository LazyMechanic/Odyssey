using Leopotam.Ecs;

namespace Odyssey {
    [EcsInject]
    sealed class ChangeLevelSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<ChangeGameLevelEvent> _changeGameLevelEventFilter = null;

        void IEcsRunSystem.Run ()
        {
            foreach (var i in _changeGameLevelEventFilter)
            {
                GameConfig.GameLevel newGameLevel = _changeGameLevelEventFilter.Components1[i].gameLevel;

                switch (newGameLevel)
                {
                    case GameConfig.GameLevel.Game:
                        ChangeLevelToGame();
                        break;
                    case GameConfig.GameLevel.Menu:
                        ChangeLevelToMenu();
                        break;
                }
            }
        }

        void ChangeLevelToGame()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<BarrierAreaMapResetEvent>();
        }

        void ChangeLevelToMenu()
        {

        }
    }
}