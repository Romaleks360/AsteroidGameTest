using System;
using System.Collections.Generic;
using Asteroids.Actors;

namespace Asteroids.Systems {
    public class DisposeSystem : ActorSystem<IActor>, IActorSystem {
        
        private readonly Dictionary<IActor, Action> _disposing = new();
        private readonly ActorFactory _factory;

        public DisposeSystem(ActorFactory factory) {
            _factory = factory;
        }
        
        protected override void OnActorAdded(IActor actor) {
            actor.Alive.Changed += OnChanged;

            _disposing.Add(actor, () => actor.Alive.Changed -= OnChanged);
            
            void OnChanged(bool state) => OnActorChangeState(actor, state);
        }
        
        private void OnActorChangeState(IActor actor, bool state) {
            if (state == false) {
                if (actor is MiddleAsteroidActor middleAsteroid)
                    CreateSmallAsteroids(middleAsteroid);
                
                if (actor is LargeAsteroidActor largeAsteroid)
                    CreateMiddleAsteroids(largeAsteroid);
                
                _factory.Dispose(actor);
            }    
        }
        
        private void CreateMiddleAsteroids(LargeAsteroidActor asteroid) {
            
        }
        
        private void CreateSmallAsteroids(MiddleAsteroidActor asteroid) {
            
        }

        protected override void OnActorRemove(IActor actor) {
            _disposing[actor].Invoke();
            _disposing.Remove(actor);
        }

        public void Update() {}
    }
}
