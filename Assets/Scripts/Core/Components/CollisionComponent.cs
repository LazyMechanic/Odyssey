using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    sealed class CollisionComponent : IEcsAutoResetComponent
    {
        public List<Collision> collisionsOnEnter;
        public List<Collision> collisionsOnExit;
        public List<Collision> collisionsOnStay;
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