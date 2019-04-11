using System;
using Unity.Entities;

[Serializable]
public struct BarrierTag : IComponentData { }

public class BarrierTagComponent : ComponentDataProxy<BarrierTag> { }