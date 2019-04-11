using System;
using UnityEngine;
using Unity.Entities;

[Serializable]
public struct AntiGravityData : IComponentData
{
    public Vector3 force;
}

public class AntiGravityComponent : ComponentDataProxy<AntiGravityData> { }