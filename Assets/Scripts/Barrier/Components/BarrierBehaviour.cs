using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBehaviour : MonoBehaviour
{
    public float damage;
    public bool isDestroyable;
    public Renderer renderer;
    [HideInInspector]
    public MaterialPropertyBlock materialPropertyBlock;
}
