using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Assertions;

public class Core : MonoBehaviour
{
    // Game core instance
    public static Core instance { get; private set; } = null;

    // Density of air
    public float density = 1.225f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        var antiGravitySystem = World.Active.GetOrCreateManager<AntiGravitySystem>();
        antiGravitySystem.Init();
    }
}
