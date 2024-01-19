using System.Collections.Generic;
using System.Linq;
using Asteroids.Actors;
using Asteroids.Systems;
using Common;
using UnityEngine;

public class Sandbox : MonoBehaviour {

    [SerializeField] private FactoryMethod[] _methods;
    
    private readonly HashSet<IActorSystem> _systems = new() {
        new InputSystem(),
        new MovementSystem(),
        new BoundsSystem(),
        new LifeTimeSystem(),
        new CollisionSystem(),
    };
     
    private void Start() {
        var factory = new ActorFactory(_methods);

        _systems.Add(new DisposeSystem(factory));
        _systems.Add(new BallisticsSystem(factory));
        
        _systems.Each(s => factory.AddListener(s));

        Enumerable.Range(0, 4).Each(i => {
            var position = Random.insideUnitCircle * 5;
            var direction = Vector3.forward * Random.Range(0, 360);
            
            factory.Create<LargeAsteroidActor>(position, direction);
        });
        
        factory.Create<PlayerShipActor>(Vector3.zero, Vector3.up);
    }

    private void Update() {
        _systems.Each(s => s.Update());
    }
}
