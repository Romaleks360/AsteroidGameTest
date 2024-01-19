using System;
using System.Collections.Generic;
using Asteroids.Actors;

namespace Asteroids.Systems {

    public interface IDisposeHandler {
        void OnDisposedActor(IActor actor);
    }
    
    public class DisposeSystem : ActorSystem<IActor>, IActorSystem {
        
        private readonly Dictionary<IActor, Action> _disposing = new();
        private readonly IDisposeHandler _disposeHandler;
        private readonly ActorFactory _factory;

        public DisposeSystem(ActorFactory factory, IDisposeHandler disposeHandler) {
            _factory = factory;
            _disposeHandler = disposeHandler;
        }
        
        protected override void OnActorAdded(IActor actor) {
            actor.Alive.Changed += OnChanged;

            _disposing.Add(actor, () => actor.Alive.Changed -= OnChanged);
            
            void OnChanged(bool state) => OnActorChangeState(actor, state);
        }
        
        private void OnActorChangeState(IActor actor, bool state) {
            if (state == false) {
                _disposeHandler.OnDisposedActor(actor);
                _factory.Dispose(actor);
            }    
        }

        protected override void OnActorRemove(IActor actor) {
            _disposing[actor]?.Invoke();
            _disposing.Remove(actor);
        }

        public void Update() {}
    }
}
