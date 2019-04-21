using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipMovementSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<AxisComponent> _axisFilter = null;
        private EcsFilter<BeatshipTagComponent, TransformComponent, RigidbodyComponent, SpeedComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run ()
        {
            Vector3 movementForce =
                //Mathf.Max(_spaceshipMovementEntities.joystickData[i].thrust * _spaceshipMovementEntities.speedData[i].maxForwardSpeed, _spaceshipMovementEntities.speedData[i].minForwardSpeed) * Vector3.forward + // <------ RIGHT COMPUTING
                _axisFilter.Components1[0].thrust *
                _beatshipFilter.Components4[0].maxForwardSpeed *
                _beatshipFilter.Components2[0].transform.forward + // <------ NOT RIGHT COMPUTING
                _axisFilter.Components1[0].horizontal *
                _beatshipFilter.Components4[0].maxSideSpeed *
                _beatshipFilter.Components2[0].transform.right;

            _beatshipFilter.Components3[0].rigidbody.AddForce(movementForce, ForceMode.Force);
        }
    }
}