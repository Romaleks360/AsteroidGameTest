using Asteroids.Actors;
using Common;
using UnityEngine;

namespace Asteroids.Systems {
    
    public enum InputAction { None, Primary, Secondary }

    public class ActionsSystem : ActorSystem<IControllableActor>, IActorSystem {

        public void Update() {
            var action = Input.GetKeyDown(KeyCode.Space) ? InputAction.Primary :
                Input.GetKeyDown(KeyCode.E) ? InputAction.Secondary : InputAction.None;
            
            Actors.Each(actor => actor.Action.Value = action);
        }
        
    }
}
