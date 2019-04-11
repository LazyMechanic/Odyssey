using System;
using Unity.Entities;

[Serializable]
public struct SpaceshipTag : IComponentData { }

public class SpaceshipTagComponent : ComponentDataProxy<SpaceshipTag> { }
