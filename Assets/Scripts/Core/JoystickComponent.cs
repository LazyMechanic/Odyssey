using System;
using Unity.Entities;

[Serializable]
public struct JoystickData : IComponentData
{
    public float horizontal;
    public float vertical;
    public float thrust;
}

public class JoystickComponent : ComponentDataProxy<JoystickData> { }
