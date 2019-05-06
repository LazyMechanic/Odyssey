using System;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class GameStateSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private GameConfig _gameConfig = null;
        private EcsFilter<ChangeGameStateEvent> _changeGameStateEventFilter = null;
        
        void IEcsRunSystem.Run () {
            foreach (var i in _changeGameStateEventFilter)
            {
                _gameConfig.gameState = _changeGameStateEventFilter.Components1[i].gameState;
                switch (_gameConfig.gameState)
                {
                    case GameConfig.GameState.Pause:
                        Time.timeScale = 0.0f;
                        break;
                    case GameConfig.GameState.Start:
                        Time.timeScale = 1.0f;
                        break;
                    case GameConfig.GameState.Exit:
                        Application.Quit();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}