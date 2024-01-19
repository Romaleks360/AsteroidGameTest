using Asteroids.Actors;
using Common;
using UnityEngine;

namespace Asteroids.Systems {
    public class LifeTimeSystem : ActorSystem<IConsumableActor>, IActorSystem {

        public void Update() {
            Actors.Each(actor => {
                if (actor.LifeEnd.Value < Time.time) {
                    Debug.Log("Disappear Actor!");
                    actor.Alive.Value = false;
                }
            });
        }
    }

}
