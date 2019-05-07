using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipCollisionSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipTagComponent, CollisionComponent, BeatshipLastVelocityComponent, BeatshipMaxCollisionAngleComponent> _beatshipFilter = null;

        void IEcsRunSystem.Run ()
        {
            ProcessCollisionEnter();
            ProcessCollisionExit();
            ProcessCollisionStay();
        }

        void ProcessCollisionEnter()
        {
            foreach (var collision in _beatshipFilter.Components2[0].collisionsOnEnter)
            {
                Vector3 collisionNormal = collision.collision.contacts[0].normal;
                Vector3 velocityOnCollision = _beatshipFilter.Components3[0].lastVelocity;

                float collisionAngle = Vector3.Angle(velocityOnCollision, -collisionNormal);

                var barrierDamageBehaviour = collision.collision.gameObject.GetComponent<BarrierDamageBehaviour>();
                if (barrierDamageBehaviour != null)
                {
                    float maxCollisionAngle = _beatshipFilter.Components4[0].maxCollisionAngle;
                    if (collisionAngle < maxCollisionAngle)
                    {

                        EntityBuilder.Instance(_world)
                                     .CreateEntity()
                                     .AddComponent<DelayDestroyObjectEvent>(
                                         out DelayDestroyObjectEvent destroyObjectEvent);

                        destroyObjectEvent.gameObject = collision.collision.gameObject;

                        EntityBuilder.Instance(_world)
                                     .CreateEntity()
                                     .AddComponent<BeatshipAddDamageEvent>(out BeatshipAddDamageEvent damageEvent);

                        float damageCoefficient = 1.0f - collisionAngle / maxCollisionAngle;
                        float damage = barrierDamageBehaviour.damage;
                        damageEvent.damage = damage * damageCoefficient;
                    }
                }
            }

            _beatshipFilter.Components2[0].collisionsOnEnter.Clear();
        }

        void ProcessCollisionExit()
        {
            _beatshipFilter.Components2[0].collisionsOnExit.Clear();
        }

        void ProcessCollisionStay()
        {
            _beatshipFilter.Components2[0].collisionsOnStay.Clear();
        }
    }
}