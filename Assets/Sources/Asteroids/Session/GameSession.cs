using System;
using System.Collections.Generic;
using Asteroids.Actors;
using Asteroids.Systems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Session {
    
    public class GameSession : IDisposeHandler {
        
        private readonly ActorFactory _factory;
        private readonly Queue<Action> _delayedTasks = new();
        public GameSession(ActorFactory factory) {
            _factory = factory;
        }

        public void OnDisposedActor(IActor actor) {
            if (actor is SmallAsteroidActor smallAsteroid)
                _factory.CreateSmallExplosion(smallAsteroid.Position.Value, smallAsteroid.Direction.Value);
            
            if (actor is MiddleAsteroidActor middleAsteroid)
                _factory.CreateAsteroids<SmallAsteroidActor>(middleAsteroid);
                
            if (actor is LargeAsteroidActor largeAsteroid)
                _factory.CreateAsteroids<MiddleAsteroidActor>(largeAsteroid);

            if (actor is PlayerShipActor playerShip) {
                _factory.CreateBigExplosion(playerShip.Position.Value, playerShip.Direction.Value);
                
                var position = Random.insideUnitCircle * 10;
                var direction = Vector3.forward * Random.Range(0, 360);
            
                _factory.Create<PlayerShipActor>(position, direction);
            }
        }
        
    }
    
}