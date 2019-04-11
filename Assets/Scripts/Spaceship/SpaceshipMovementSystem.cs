using System;
using UnityEngine;
using Unity.Entities;

public class SpaceshipMovementSystem : ComponentSystem
{
    struct SpaceshipMovementFilter
    {
        public ComponentArray<Rigidbody> rigidbody;
        public ComponentDataArray<SpaceshipTag> tag;
        public ComponentDataArray<SpeedData> speedData;
        public ComponentDataArray<JoystickData> joystickData;
        public ComponentDataArray<AntiGravityData> antiGravityData;
        public readonly int Length;
    }

    [Inject] private SpaceshipMovementFilter _spaceshipMovementEntities;

    protected override void OnUpdate()
    {
        for (int i = 0; i < _spaceshipMovementEntities.Length; ++i)
        {
            Vector3 movementForce =
                //Mathf.Max(_spaceshipMovementEntities.joystickData[i].thrust * _spaceshipMovementEntities.speedData[i].maxForwardSpeed, _spaceshipMovementEntities.speedData[i].minForwardSpeed) * Vector3.forward + // <------ RIGHT COMPUTING
                _spaceshipMovementEntities.joystickData[i].thrust * _spaceshipMovementEntities.speedData[i].maxForwardSpeed * Vector3.forward + // <------ NOT RIGHT COMPUTING
                _spaceshipMovementEntities.joystickData[i].horizontal * _spaceshipMovementEntities.speedData[i].maxSideSpeed * Vector3.right +
                _spaceshipMovementEntities.joystickData[i].vertical * _spaceshipMovementEntities.speedData[i].maxVerticalSpeed * Vector3.up;

            // Delete movement force from spaceship
            _spaceshipMovementEntities.rigidbody[i].AddForce(movementForce, ForceMode.Force);

            _spaceshipMovementEntities.rigidbody[i].AddForce(_spaceshipMovementEntities.antiGravityData[i].force, ForceMode.Force);
        }
    }
}
