using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Odyssey
{
    public class CollisionState
    {
        public Collision collision = null;
        public Vector3 velocityOnCollision = Vector3.zero;
    }

    public class CollisionBehaviour : MonoBehaviour
    {
        public List<CollisionState> collisionsOnEnter = new List<CollisionState>();
        public List<CollisionState> collisionsOnExit = new List<CollisionState>();
        public List<CollisionState> collisionsOnStay = new List<CollisionState>();
        public Collider collider;
        public Rigidbody rigidbody;

        void Start()
        {
            if (collider == null)
                collider = GetComponent<Collider>();
            Assert.IsNotNull(collider, "Collider not found");

            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(rigidbody, "Rigidbody not found");
        }

        void OnCollisionEnter(Collision other)
        {
            collisionsOnEnter.Add(new CollisionState()
            {
                collision = other,
                velocityOnCollision = rigidbody.velocity
            });
        }

        void OnCollisionExit(Collision other)
        {
            collisionsOnExit.Add(new CollisionState()
            {
                collision = other,
                velocityOnCollision = rigidbody.velocity
            });
        }

        void OnCollisionStay(Collision other)
        {
            collisionsOnStay.Add(new CollisionState()
            {
                collision = other,
                velocityOnCollision = rigidbody.velocity
            });
        }
    }
}
