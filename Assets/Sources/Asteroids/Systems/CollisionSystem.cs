using Asteroids.Actors;
using Common;
using UnityEngine;

namespace Asteroids.Systems {
    public class CollisionSystem : ActorSystem<IActor>, IActorSystem {
        private float _invincibleTime = 1f;
        
        public void Update() {

            if ((_invincibleTime -= Time.deltaTime) > 0) return;
            
            Actors.Each(a => {
                Actors.Each(t => {
                    if (a != t && HasCollision(a, t)) {

                        if (a is RocketActor && t is AsteroidActor) {
                            a.Alive.Value = false;
                            t.Alive.Value = false;
                        }
                        
                        if (a is RocketActor rocket && t is PlayerShipActor) {
                            a.Alive.Value = rocket.Surrogate.Value == t;
                            t.Alive.Value = rocket.Surrogate.Value == t;
                        }
                        
                        if (a is AsteroidActor && t is PlayerShipActor) {
                            a.Alive.Value = false;
                            t.Alive.Value = false;
                        }
                    }
                });
            });
        }
        
        private bool HasCollision(IActor source, IActor target) {
            return source.Collider.bounds.Intersects(target.Collider.bounds);
        }
    }
}
