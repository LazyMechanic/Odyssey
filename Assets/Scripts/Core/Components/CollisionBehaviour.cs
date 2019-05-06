using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace Odyssey
{
    public class CollisionBehaviour : MonoBehaviour
    {
        public List<Collision> collisionsOnEnter = new List<Collision>();
        public List<Collision> collisionsOnExit = new List<Collision>();
        public List<Collision> collisionsOnStay = new List<Collision>();
        public Collider collider;

        void Start()
        {
            collider = GetComponent<Collider>();
            Assert.IsNotNull(collider, "Collider not found");
        }

        void OnCollisionEnter(Collision other)
        {
            collisionsOnEnter.Add(other);
        }

        void OnCollisionExit(Collision other)
        {
            collisionsOnExit.Add(other);
        }

        void OnCollisionStay(Collision other)
        {
            collisionsOnStay.Add(other);
        }
    }
}
