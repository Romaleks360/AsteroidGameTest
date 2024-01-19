using System;
using System.Collections.Generic;
using Asteroids.Actors;
using UnityEngine;

namespace Asteroids.Systems {
    
    public class BallisticsSystem : ActorSystem<IControllableActor>, IActorSystem {

        private readonly Dictionary<IControllableActor, Action> _disposing = new();
        private readonly IActorFactory _actorFactory;

        public BallisticsSystem(IActorFactory actorFactory) {
            _actorFactory = actorFactory;
        }
        
        protected override void OnActorAdded(IControllableActor actor) {
            actor.Action.Changed += OnChanged;

            _disposing.Add(actor, () => actor.Action.Changed -= OnChanged);
            
            void OnChanged(InputAction action) => OnActorChangeState(actor, action);
        }
        
        private void OnActorChangeState(IControllableActor actor, InputAction action) {
            if (action == InputAction.Primary) {
                _actorFactory.Child<RocketActor>(actor, actor.Position.Value, Quaternion.Euler(actor.Aim.Value).eulerAngles);
            }    
        }

        protected override void OnActorRemove(IControllableActor actor) {
            _disposing[actor].Invoke();
            _disposing.Remove(actor);
        }

        public void Update() {}
    }
}
