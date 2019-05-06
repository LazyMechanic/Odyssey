using Leopotam.Ecs;
using UnityEngine;

namespace Odyssey {
    [EcsInject]
    sealed class BeatshipCollisionSystem : IEcsRunSystem {
        // Auto-injected fields.
        EcsWorld _world = null;

        private EcsFilter<BeatshipTagComponent, CollisionComponent, RigidbodyComponent, BeatshipHealthComponent> _beatshipFilter = null;
        
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
                Vector3 collisionNormal = collision.contacts[0].normal;
                Vector3 rigidbodyVelocity = _beatshipFilter.Components3[0].rigidbody.velocity;

                float collisionAngle = Vector3.Angle(rigidbodyVelocity, -collisionNormal);

                var barrierDamageBehaviour = collision.gameObject.GetComponent<BarrierDamageBehaviour>();
                if (barrierDamageBehaviour != null)
                {
                    float maxCollisionAngle = _beatshipFilter.Components4[0].maxCollisionAngle;
                    if (collisionAngle < maxCollisionAngle)
                    {
                        float damageCoefficient = 1.0f - collisionAngle / maxCollisionAngle;
                        float damage = barrierDamageBehaviour.damage;
                        _beatshipFilter.Components4[0].health -= damage * damageCoefficient;

                        EntityBuilder.Instance(_world)
                                     .CreateEntity()
                                     .AddComponent<DelayDestroyObjectEvent>(
                                         out DelayDestroyObjectEvent destroyObjectEvent);

                        destroyObjectEvent.gameObject = collision.gameObject;

                        if (_beatshipFilter.Components4[0].health <= 0.0f)
                        {

                            // TODO
                        }
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