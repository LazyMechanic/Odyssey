using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsIgnoreInFilter]
    sealed class BeatshipSpawnerBehaviour : MonoBehaviour
    {
        public float freeFlyRadius;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, freeFlyRadius);
        }
    }
}