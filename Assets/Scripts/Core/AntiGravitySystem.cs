using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class AntiGravitySystem : ComponentSystem
{
    struct AntiGravityFilter
    {
        public ComponentDataArray<PIDData> pid;
        public ComponentDataArray<SpaceshipTag> tag;
        public ComponentDataArray<Position> position;
        public ComponentDataArray<SpeedData> speedData;
        public ComponentDataArray<AntiGravityData> antiGravityData;
        public ComponentDataArray<SpaceshipAltitudeData> spaceshipAltitude;
        public readonly int Length;
    }

    [Inject] private AntiGravityFilter _antiGravityEntities;

    public void Init()
    {
        Debug.Log("Entities length = " + _antiGravityEntities.Length);
        for (int i = 0; i < _antiGravityEntities.Length; ++i)
        {
            PIDData pidData = new PIDData();
            pidData.value = 0;
            pidData.lastError = 0;
            pidData.integral = 0;

            if (_antiGravityEntities.pid[i].kd == 0.0f)
                pidData.kd = 1.0f;
            if (_antiGravityEntities.pid[i].ki == 0.0f)
                pidData.ki = 0.05f;
            if (_antiGravityEntities.pid[i].kp == 0.0f)
                pidData.kp = 0.2f;

            Debug.Log("pid.kd = " + _antiGravityEntities.pid[i].kd);
            Debug.Log("pid.ki = " + _antiGravityEntities.pid[i].ki);
            Debug.Log("pid.kp = " + _antiGravityEntities.pid[i].kp);

            _antiGravityEntities.pid[i] = pidData;
        }
    }

    protected override void OnStartRunning()
    {
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < _antiGravityEntities.Length; ++i)
        {
            _antiGravityEntities.pid[i] = ComputePID(_antiGravityEntities.pid[i],
                ComputePIDError(_antiGravityEntities.position[i], _antiGravityEntities.spaceshipAltitude[i]));

            _antiGravityEntities.antiGravityData[i] =
                ComputeAntiGravity(_antiGravityEntities.pid[i], _antiGravityEntities.speedData[i]);
        }
    }

    private AntiGravityData ComputeAntiGravity(PIDData pid, SpeedData speedData)
    {
        AntiGravityData result = new AntiGravityData();
        float forceMagnitude = speedData.maxVerticalSpeed;
        result.force = forceMagnitude * pid.value * Vector3.up;

        return result;
    }

    private PIDData ComputePID(PIDData oldValue, float pidError)
    {
        PIDData pidData = oldValue;

        float derivative = (pidError - pidData.lastError) / Time.fixedDeltaTime;
        pidData.integral += pidError * Time.fixedDeltaTime;
        pidData.lastError = pidError;

        pidData.value =
            Mathf.Clamp01(pidData.kp * pidError + pidData.ki * pidData.integral + pidData.kd * derivative);

        return pidData;
    }

    private float ComputePIDError(Position position, SpaceshipAltitudeData targetAltitude)
    {
        return targetAltitude.value - GetAltitude(position.Value);
    }

    private float GetAltitude(Vector3 position)
    {
        RaycastHit hit;
        Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity);

        return hit.distance;
    }
}
