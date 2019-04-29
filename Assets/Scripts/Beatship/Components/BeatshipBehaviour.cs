using UnityEngine;

namespace Odyssey {
    sealed class BeatshipBehaviour : MonoBehaviour
    {
        public float viewRadius;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, viewRadius);
        }
    }
}