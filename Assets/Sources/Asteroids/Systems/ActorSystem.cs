using System.Collections.Generic;
using Asteroids.Actors;

namespace Asteroids.Systems {
    
    public abstract class ActorSystem<TActor> where TActor : IActor {

        protected HashSet<TActor> Actors { get; } = new HashSet<TActor>();
        
        public void AddActor(IActor actor) {
            if (actor is TActor systemActor) {
                Actors.Add(systemActor);

                OnActorAdded(systemActor);
            }
        }
        
        protected virtual void OnActorAdded(TActor actor) {}

        public void RemoveActor(IActor actor) {
            if (actor is TActor systemActor) {
                OnActorRemove(systemActor);
                
                Actors.Remove(systemActor);
            }
        }
        
        protected virtual void OnActorRemove(TActor systemActor) {}
    }

}
