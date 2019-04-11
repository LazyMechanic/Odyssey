using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
public struct PIDData : IComponentData
{
    // Proportional constant (counters current delta)
    public float kp;

    // Integral constant (counters cumulated delta)
    public float ki;

    // Derivative constant (fights oscillation)
    public float kd;

    // rrent control value
    public float value;

    public float lastError;

    public float integral;
}

public class PIDComponent : ComponentDataProxy<PIDData> { }