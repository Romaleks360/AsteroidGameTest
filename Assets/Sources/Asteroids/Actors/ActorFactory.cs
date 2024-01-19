using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using UnityEngine;

namespace Asteroids.Actors {
    
    public interface IActorFactory {
        TActor Create<TActor>(Vector3 position, Vector3 direction) where TActor : class, IActor;
        TActor Child<TActor>(IActor parent, Vector3 position, Vector3 direction) where TActor : class, IConsumableActor;
    }
    
    public class ActorFactory : IActorFactory {
        
        private readonly FactoryMethod[] _methods;

        private readonly HashSet<IActorSystem> _systems = new();
        
        public ActorFactory(FactoryMethod[] methods) {
            _methods = methods;
        }

        public void AddListener(IActorSystem system) {
            _systems.Add(system);
        }

        private bool TryGetMethod<TActor>(out FactoryMethod method) where TActor: class, IActor {
            method = _methods.FirstOrDefault(m => m.ActorIs<TActor>());

            return method != null;
        }
        
        private bool TryGetMethod(IActor actor, out FactoryMethod method) {
            method = _methods.FirstOrDefault(m => m.ActorIs(actor.GetType()));

            return method != null;
        }
        
        public TActor Create<TActor>(Vector3 position, Vector3 direction) where TActor : class, IActor {

            if (!TryGetMethod<TActor>(out var method)) throw new NullReferenceException();

            var actor = method.Create(position, direction);
            
            _systems.Each(s => s.AddActor(actor));
            
            return actor as TActor;
        }

        public TActor Child<TActor>(IActor parent, Vector3 position, Vector3 direction) where TActor : class, IConsumableActor {
            var actor = Create<TActor>(position, direction);
            
            actor.Surrogate.Value = parent;
            
            return actor;
        }

        public void Dispose(IActor actor) {
            if (TryGetMethod(actor, out var method)) {
                _systems.Each(s => s.RemoveActor(actor));

                method.DestroyActor(actor);
            }
            
        }
    }

}
