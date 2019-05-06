using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipRotationSystem : IEcsRunSystem
    {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<AxisComponent> _axisFilter = null;
        private EcsFilter<BeatshipTagComponent, BeatshipModelTransform, BeatshipRotationComponent, TransformComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run()
        {
            RotateOnRollAxis();
            RotateOnPitchAxis();
        }

        void RotateOnRollAxis()
        {
            float inputAxis = _axisFilter.Components1[0].horizontal;
            float angleLimit = _beatshipFilter.Components3[0].rollLimit;
            float rotationSpeed = _beatshipFilter.Components3[0].rollRotationSpeed;
            Vector3 rotationAxis = _beatshipFilter.Components2[0].rollTransform.forward;

            Quaternion targetRotation = ComputeRotation(inputAxis, angleLimit, rotationAxis);
            Rotate(_beatshipFilter.Components2[0].rollTransform, targetRotation, rotationSpeed);
        }

        void RotateOnPitchAxis()
        {
            float inputAxis = -_axisFilter.Components1[0].vertical;
            float angleLimit = _beatshipFilter.Components3[0].pitchLimit;
            float rotationSpeed = _beatshipFilter.Components3[0].pitchRotationSpeed;
            Vector3 rotationAxis = _beatshipFilter.Components2[0].pitchTransform.right;
            AnimationCurve curve = _beatshipFilter.Components3[0].pitchCurve;

            Quaternion currentPitchRotation = _beatshipFilter.Components2[0].pitchTransform.rotation;
            Quaternion originalRotation = _beatshipFilter.Components4[0].transform.rotation;

            float angleBetweenCurrentAndOriginalRotation = Quaternion.Angle(currentPitchRotation, originalRotation);
            float pitchCurveFunc = curve.Evaluate(angleBetweenCurrentAndOriginalRotation / angleLimit);

            // TODO: rotate pitch axis

            //Quaternion targetRotation = ComputeRotation(inputAxis * pitchCurveFunc, angleLimit, rotationAxis);
            //Rotate(_beatshipFilter.Components2[0].pitchTransform, targetRotation, rotationSpeed);
        }

        Quaternion ComputeRotation(float inputAxis, float angleLimit, Vector3 rotationAxis)
        {
            float targetAngle = inputAxis * angleLimit;

            return Quaternion.AngleAxis(targetAngle, rotationAxis);
        }

        void Rotate(Transform transform, Quaternion targetRotation, float rotationSpeed)
        {
            Quaternion currentBeatshipRotation = transform.rotation;
            transform.rotation =
                Quaternion.Lerp(currentBeatshipRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}