using System;
using UnityEngine;
using Unity.Entities;

public class InputSystem : ComponentSystem
{
    struct InputFilter
    {
        public ComponentDataArray<JoystickData> joystickData;
        public readonly int Length;
    }

    [Inject] private InputFilter _inputEntities;

    protected override void OnUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float thrust = Input.GetAxis("Thrust");

        for (int i = 0; i < _inputEntities.Length; ++i)
        {
            var tempInput = _inputEntities.joystickData[i];

            tempInput.horizontal = horizontal;
            tempInput.vertical = vertical;
            tempInput.thrust = thrust;

            _inputEntities.joystickData[i] = tempInput;
        }
    }
}
