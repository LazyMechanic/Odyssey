using Leopotam.Ecs;
using Odyssey;
using UnityEngine;

namespace Odyssey
{
    sealed class Startup : MonoBehaviour
    {
        private GameConfig _gameConfig;

        private EcsWorld _world;

        private EcsSystems _initSystems;

        private EcsSystems _generalUpdateSystems;

        private EcsSystems _inGameUpdateSystems;
        private EcsSystems _inGameFixedUpdateSystems;

        private EcsSystems _inMenuUpdateSystems;
        private EcsSystems _inMenuFixedUpdateSystems;

        void OnEnable ()
        {
            _gameConfig = new GameConfig();
            _gameConfig.gameState = GameConfig.GameState.Start;
            _gameConfig.gameLevel = GameConfig.GameLevel.Menu;

            _world = new EcsWorld();

            _initSystems = new EcsSystems(_world);
            _generalUpdateSystems = new EcsSystems(_world);

            _inGameUpdateSystems = new EcsSystems(_world);
            _inGameFixedUpdateSystems = new EcsSystems(_world);

            _inMenuUpdateSystems = new EcsSystems(_world);
            _inMenuFixedUpdateSystems = new EcsSystems(_world);

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_generalUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_inGameUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_inGameFixedUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_inMenuUpdateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_inMenuFixedUpdateSystems);
#endif

            // Create shared systems
            // Update systems
            var pidSystem = new PidSystem();
            var antiGravitySystem = new AntiGravitySystem();
            var antiGravityApplySystem = new AntiGravityApplySystem();

            // Fixed update systems
            var axisSystem = new AxisSystem();
            var attachSystem = new AttachSystem();
            var detachSystem = new DetachSystem();
            var delayedDestroyObjectSystem = new DelayedDestroyObjectSystem();

            _generalUpdateSystems
                .Add(new GameStateSystem())
                .Add(new GameLevelSystem())
                .Add(new ChangeLevelSystem())
                .Inject(_gameConfig)
                .Initialize();

            _inGameUpdateSystems
                .Add(axisSystem)
                .Add(new BarrierAreaMapResetSystem())
                .Add(new LevelGenerateSystem())
                .Add(new LevelClearSystem())
                .Add(new BarrierAreaSpawnSystem())
                .Add(new BarrierPatternGenerateSystem())
                .Add(new BarrierAreaDestroySystem())
                .Add(new BeatshipRotationSystem())
                .Add(attachSystem)
                .Add(detachSystem)
                .Add(delayedDestroyObjectSystem)
                .Inject(_gameConfig)
                .Initialize();

            _inGameFixedUpdateSystems
                .Add(new BeatshipAltitudeSystem())
                .Add(pidSystem)
                .Add(antiGravitySystem)
                .Add(antiGravityApplySystem)
                .Add(new BeatshipMovementSystem())
                .Add(new BeatshipPositionRecoverySystem())
                .Add(new BeatshipCollisionSystem())
                .Add(new BeatshipLastVelocityUpdateSystem())
                .Inject(_gameConfig)
                .Initialize();

            _inMenuUpdateSystems
                .Add(axisSystem)
                .Add(attachSystem)
                .Add(detachSystem)
                .Add(delayedDestroyObjectSystem)
                .Inject(_gameConfig)
                .Initialize();

            _inMenuFixedUpdateSystems
                .Add(pidSystem)
                .Add(antiGravitySystem)
                .Add(antiGravityApplySystem)
                .Inject(_gameConfig)
                .Initialize();

            _initSystems
                .Add(new AxisInitSystem())
                .Add(new AntiGravityInitSystem())
                .Add(new PidInitSystem())
                .Add(new BeatshipSpawnerInitSystem())
                .Add(new BeatshipInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new BarrierAreaMapInitSystem())
                .Add(new BarrierAreaContainerInitSystem())
                .Inject(_gameConfig)
                .Initialize();
        }

        void Update ()
        {
            RunUpdateSystems();
            _world.RemoveOneFrameComponents();
        }

        void RunUpdateSystems()
        {
            _generalUpdateSystems.Run();

            if (_gameConfig.gameLevel == GameConfig.GameLevel.Game)
                _inGameUpdateSystems.Run();
            else if (_gameConfig.gameLevel == GameConfig.GameLevel.Menu)
                _inMenuUpdateSystems.Run();
        }

        void FixedUpdate()
        {
            RunFixedUpdateSystems();
        }

        void RunFixedUpdateSystems()
        {
            if (_gameConfig.gameLevel == GameConfig.GameLevel.Game)
                _inGameFixedUpdateSystems.Run();
            else if (_gameConfig.gameLevel == GameConfig.GameLevel.Menu)
                _inMenuFixedUpdateSystems.Run();
        }

        void OnDisable ()
        {
            _initSystems.Dispose();
            _initSystems = null;

            _inGameUpdateSystems.Dispose();
            _inGameUpdateSystems = null;

            _inGameFixedUpdateSystems.Dispose ();
            _inGameFixedUpdateSystems = null;

            _inMenuUpdateSystems.Dispose();
            _inMenuUpdateSystems = null;

            _inMenuFixedUpdateSystems.Dispose();
            _inMenuFixedUpdateSystems = null;

            _world.Dispose ();
            _world = null;
        }

        public void PauseGame()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<ChangeGameStateEvent>(out ChangeGameStateEvent changeGameStateEvent);

            changeGameStateEvent.gameState = GameConfig.GameState.Pause;
        }

        public void StartGame()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<ChangeGameStateEvent>(out ChangeGameStateEvent changeGameStateEvent);

            changeGameStateEvent.gameState = GameConfig.GameState.Start;
        }

        public void ExitGame()
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<ChangeGameStateEvent>(out ChangeGameStateEvent changeGameStateEvent);

            changeGameStateEvent.gameState = GameConfig.GameState.Exit;
        }

        public void ChangeLevel(GameConfig.GameLevel level)
        {
            EntityBuilder.Instance(_world)
                         .CreateEntity()
                         .AddComponent<ChangeGameLevelEvent>(out ChangeGameLevelEvent changeGameLevelEvent);

            changeGameLevelEvent.gameLevel = level;
        }
    }
}