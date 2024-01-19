using Asteroids.Actors;
using Common;
using UnityEngine;

namespace Asteroids.Systems {
    public class AimSystem : ActorSystem<IControllableActor>, IActorSystem {

        public void Update() {
            var input = new Vector2(Input.GetAxis("Horizontal") * -1, Mathf.Clamp01(Input.GetAxis("Vertical")));
            
            Actors.Each(actor => {
                ControlActor(actor, input);
            });
        }
        
        private static void ControlActor(IControllableActor actor, Vector2 input) {
            actor.Aim.Value += Vector3.forward * input.x;
            
            if (input.y > 0) {
                var angle = Quaternion.Angle(Quaternion.Euler(actor.Direction.Value), Quaternion.Euler(actor.Aim.Value));
                actor.Direction.Value = actor.Aim.Value;
                
                if(angle > 90)
                    actor.Speed.Value *= .5f;
            }

            var acceleration = actor.MaxSpeed.Value * (input.y > 0 ?  .5f: -.1f);
                
            actor.Speed.Value = Mathf.Clamp(actor.Speed.Value + Time.deltaTime * acceleration, 1, actor.MaxSpeed.Value);
        }
    }
}
