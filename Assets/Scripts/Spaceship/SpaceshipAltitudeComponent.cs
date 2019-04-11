using System;
using Unity.Entities;

[Serializable]
public struct SpaceshipAltitudeData : IComponentData
{
    public float value;
}
 
public class SpaceshipAltitudeComponent : ComponentDataProxy<SpaceshipAltitudeData> { }
