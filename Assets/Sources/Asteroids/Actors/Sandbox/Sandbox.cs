using Asteroids.Actors;
using Asteroids.Session;
using Asteroids.Systems;
using Common;
using UnityEngine;

public class Sandbox : MonoBehaviour {

    [SerializeField] private FactoryMethod[] _methods;

    private IActorSystem[] _systems;
    private GameSession _session;

    private void Start() {
        var factory = new ActorFactory(_methods);
        _session = new GameSession(factory);

        _systems = new IActorSystem[] {
            new AimSystem(),
            new ActionsSystem(),
            new MovementSystem(),
            new BoundsSystem(),
            new LifeTimeSystem(),
            new CollisionSystem(),
            new DisposeSystem(factory, _session),
            new BallisticsSystem(factory)
        };
        
        _systems.Each(s => factory.AddListener(s));
        
        _session.StartGame();
    }

    private void Update() => _systems.Each(s => s.Update());
}
