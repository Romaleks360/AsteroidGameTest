using Asteroids.Actors;
using Common;
using UnityEngine;

namespace Asteroids.Systems {
    
    public enum InputAction { None, Primary, Secondary }
    
    public class InputSystem : ActorSystem<IControllableActor>, IActorSystem {

        public void Update() {
            var input = new Vector2(Input.GetAxis("Horizontal") * -1, Mathf.Clamp01(Input.GetAxis("Vertical")));
            var action = Input.GetKeyDown(KeyCode.Space) ? InputAction.Primary :
                Input.GetKeyDown(KeyCode.E) ? InputAction.Secondary : InputAction.None;
            
            Actors.Each(actor => {
                ControlActor(actor, input, action);
            });
        }
        
        private static void ControlActor(IControllableActor actor, Vector2 input, InputAction action) {
            actor.Aim.Value += Vector3.forward * input.x;
            actor.Action.Value = action;
            
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
