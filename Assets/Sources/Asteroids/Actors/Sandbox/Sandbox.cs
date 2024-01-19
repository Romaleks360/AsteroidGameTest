using System.Collections.Generic;
using System.Linq;
using Asteroids.Actors;
using Asteroids.Session;
using Asteroids.Systems;
using Common;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sandbox : MonoBehaviour {

    [SerializeField] private FactoryMethod[] _methods;
    
    private readonly HashSet<IActorSystem> _systems = new() {
        new AimSystem(),
        new ActionsSystem(),
        new MovementSystem(),
        new BoundsSystem(),
        new LifeTimeSystem(),
        new CollisionSystem(),
    };
    private GameSession _session;

    private void Start() {
        var factory = new ActorFactory(_methods);
        _session = new GameSession(factory);
        
        _systems.Add(new DisposeSystem(factory, _session));
        _systems.Add(new BallisticsSystem(factory));
        
        _systems.Each(s => factory.AddListener(s));

        Enumerable.Range(0, 4).Each(i => {
            var position = Random.insideUnitCircle * 10;
            var direction = Vector3.forward * Random.Range(0, 360);
            
            factory.Create<LargeAsteroidActor>(position, direction);
        });
        
        factory.Create<PlayerShipActor>(Vector3.zero, Vector3.up);
    }

    private void Update() {
        _systems.Each(s => s.Update());
    }
}
