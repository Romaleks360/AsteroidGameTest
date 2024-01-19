using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Systems;
using Common;
using Common.Observables;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asteroids.Actors {

    public interface IActorSystem {
        void AddActor(IActor actor);
        void Update();
        void RemoveActor(IActor actor);
    }

    public interface IActor {
        Collider Collider { get; }
        public IObservableValue<bool> Alive { get; }
    }

    public interface IMoveableActor : IActor {
        IObservableValue<Vector3> Position { get; }
        IObservableValue<Vector3> Direction { get; }
        IObservableValue<float> Speed { get; }
        IObservableValue<float> MaxSpeed { get; }
    }

    public interface IConsumableActor : IMoveableActor {
        IObservableValue<float> LifeEnd { get; }
        IObservableValue<IActor> Surrogate { get; }
    }

    public interface IControllableActor : IMoveableActor {
        
        IObservableValue<Vector2> Input { get; }
        IObservableValue<InputAction> Action { get; }
        IObservableValue<Vector3> Aim { get; }
        
    }

    public interface IActorDisposer {
        void Dispose(IActor actor);
    }

    public interface IActorFactory {
        TActor Create<TActor>(Vector3 position, Vector3 direction) where TActor : class, IActor;
        TActor Child<TActor>(IActor parent, Vector3 position, Vector3 direction) where TActor : class, IConsumableActor;
    }

    public class ActorFactory : IActorDisposer, IActorFactory {
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
            _systems.Each(s => s.RemoveActor(actor));

            if (actor is MonoBehaviour mono) {
                Object.Destroy(mono.gameObject);
            }
            // TODO: Dispose
        }
    }
}


