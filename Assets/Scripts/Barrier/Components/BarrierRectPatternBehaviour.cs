using UnityEngine;

namespace Odyssey
{
    sealed class BarrierRectPatternBehaviour : MonoBehaviour
    {
        public Vector3 size = new Vector3(0, 20, 0);
        public BarrierPatternFillType fillType = BarrierPatternFillType.Random;
        [Range(0.0f, 1.0f)]
        public float density = 0.5f;

#if DEBUG || UNITY_EDITOR
        void OnDrawGizmos()
        {
            Vector3 position = transform.position;
            position.y += size.y / 2;
            Gizmos.DrawWireCube(position, size);
        }
#endif
    }
}