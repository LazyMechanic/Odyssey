using Leopotam.Ecs;
using Odyssey;
using UnityEngine;

namespace Odyssey
{
    sealed class Bootstrap : MonoBehaviour {
        private EcsWorld _world;
        private EcsSystems _initSystems;
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        void OnEnable ()
        {
            _world = new EcsWorld();
            _initSystems = new EcsSystems(_world);
            _updateSystems = new EcsSystems(_world);
            _fixedUpdateSystems = new EcsSystems(_world);

#if UNITY_EDITOR
            Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_updateSystems);
            Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_fixedUpdateSystems);
#endif

            _updateSystems
                .Add(new AxisSystem())
                .Add(new BarrierAreaSpawnSystem())
                .Add(new BarrierPatternGenerateSystem())
                .Add(new BarrierAreaDestroySystem())
                .Initialize();

            _fixedUpdateSystems
                .Add(new BeatshipAltitudeSystem())
                .Add(new PidSystem())
                .Add(new AntiGravitySystem())
                .Add(new AntiGravityApplySystem())
                .Add(new BeatshipMovementSystem())
                .Add(new PositionRecoverySystem())
                .Initialize();

            _initSystems
                .Add(new AxisInitSystem())
                .Add(new AntiGravityInitSystem())
                .Add(new PidInitSystem())
                .Add(new BeatshipSpawnInitSystem())
                .Add(new BeatshipInitSystem())
                .Add(new PlayerInitSystem())
                .Add(new BarrierAreaMapInitSystem())
                .Initialize();
        }

        void Update ()
        {
            _updateSystems.Run();
            // Optional: One-frame components cleanup.
            _world.RemoveOneFrameComponents();
        }

        void FixedUpdate()
        {
            _fixedUpdateSystems.Run();
        }

        void OnDisable ()
        {
            _initSystems.Dispose();
            _initSystems = null;
            _updateSystems.Dispose();
            _updateSystems = null;
            _fixedUpdateSystems.Dispose ();
            _fixedUpdateSystems = null;
            _world.Dispose ();
            _world = null;
        }
    }
}