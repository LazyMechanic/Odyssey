using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Odyssey
{

    public class GameConfig
    {
        public enum GameLevel
        {
            Game,
            Menu
        }

        public enum GameState
        {
            Start,
            Pause,
            Exit
        }

        public GameLevel gameLevel;

        public GameState gameState;
        public GameState lastGameState;
    }
}
