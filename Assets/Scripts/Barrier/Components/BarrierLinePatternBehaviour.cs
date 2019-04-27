using UnityEngine;

namespace Odyssey {
    sealed class BarrierLinePatternBehaviour : MonoBehaviour
    {
        public float size = 20.0f;
        public BarrierPatternFillType fillType = BarrierPatternFillType.Random;
        [Range(0.0f, 1.0f)]
        public float density = 0.5f;

#if DEBUG || UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position - transform.forward * (size / 2), transform.position + transform.forward * (size / 2));
        }
#endif
    }
}