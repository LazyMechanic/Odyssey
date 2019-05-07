using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class CollisionComponent : IEcsAutoResetComponent
    {
        public List<CollisionState> collisionsOnEnter;
        public List<CollisionState> collisionsOnExit;
        public List<CollisionState> collisionsOnStay;
        public Collider collider;

        public void Reset()
        {
            collisionsOnEnter.Clear();
            collisionsOnEnter = null;

            collisionsOnExit.Clear();
            collisionsOnExit = null;

            collisionsOnStay.Clear();
            collisionsOnStay = null;

            collider = null;
        }
    }
}