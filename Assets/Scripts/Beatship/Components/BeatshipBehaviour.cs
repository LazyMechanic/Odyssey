using UnityEngine;

namespace Odyssey {
    sealed class BeatshipBehaviour : MonoBehaviour
    {
        public AnimationCurve viewOpacityCurve;
        public AnimationCurve pitchCurve;
        public float viewRadius;
        public float rollLimit;
        public float pitchLimit;
        public float rollRotationSpeed;
        public float pitchRotationSpeed;

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(viewRadius * 2, viewRadius * 2, viewRadius * 2));

            float lineLength = 3.0f;

            Vector3 rollLeftPosition = transform.position +
                                       Vector3.up * lineLength * Mathf.Sin(rollLimit * Mathf.Deg2Rad) +
                                       Vector3.left * lineLength * Mathf.Cos(rollLimit * Mathf.Deg2Rad);

            Vector3 rollRightPosition = Vector3.Reflect(rollLeftPosition, Vector3.right);

            Vector3 pitchBottomPosition = transform.position +
                                          Vector3.up * lineLength * Mathf.Sin(pitchLimit * Mathf.Deg2Rad) +
                                          Vector3.forward * lineLength * Mathf.Cos(pitchLimit * Mathf.Deg2Rad);

            Vector3 pitchTopPosition = Vector3.Reflect(pitchBottomPosition, Vector3.up);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, rollLeftPosition);
            Gizmos.DrawLine(transform.position, rollRightPosition);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, pitchBottomPosition);
            Gizmos.DrawLine(transform.position, pitchTopPosition);

            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * lineLength);
            Gizmos.DrawLine(transform.position, transform.position + transform.right * lineLength);
            Gizmos.DrawLine(transform.position, transform.position + (-transform.right) * lineLength);
        }
    }
}