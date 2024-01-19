using Asteroids.Systems;
using Common.Observables;
using UnityEngine;

namespace Asteroids.Actors {

    public interface IActorSystem {
        void AddActor(IActor actor);
        void Update();
        void RemoveActor(IActor actor);
    }
    
    public interface IActorComponent {}

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
}


