using System;
using Unity.Entities;

[Serializable]
public struct SpeedData : IComponentData
{
    public float maxForwardSpeed;
    public float minForwardSpeed;
    public float maxSideSpeed;
    public float maxVerticalSpeed;
}

public class SpeedComponent : ComponentDataProxy<SpeedData> { }