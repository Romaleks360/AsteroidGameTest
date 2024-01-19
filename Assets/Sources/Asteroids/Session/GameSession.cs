using System.Linq;
using Asteroids.Actors;
using Asteroids.Systems;
using Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroids.Session {
    
    public class GameSession : IDisposeHandler {
        
        private readonly ActorFactory _factory;
        
        public GameSession(ActorFactory factory) {
            _factory = factory;
        }
        
        public void StartGame() {
            Enumerable.Range(0, 4).Each(i => {
                var position = Random.insideUnitCircle * 10;
                var direction = Vector3.forward * Random.Range(0, 360);
            
                _factory.Create<LargeAsteroidActor>(position, direction);
            });
        
            _factory.Create<PlayerShipActor>(Vector3.zero, Vector3.up);
        }

        public void OnActorsCountChanged(int actorsCount) {
            if (actorsCount == 0) {
                Debug.Log("Stage clear! Remove stage and wait next wave..");
            }
        }
        
        public void OnDisposedActor(IActor actor) {

            if (actor is SmallAsteroidActor smallAsteroid) {
                _factory.CreateSmallExplosion(smallAsteroid.Position.Value, smallAsteroid.Direction.Value);
                // Score++
            }
            
            if (actor is MiddleAsteroidActor middleAsteroid) {
                _factory.CreateAsteroids<SmallAsteroidActor>(middleAsteroid);
                // Score++
            }
                
            if (actor is LargeAsteroidActor largeAsteroid) {
                _factory.CreateAsteroids<MiddleAsteroidActor>(largeAsteroid);
                // Score++
            }

            if (actor is PlayerShipActor playerShip) {
                // PlayerLives--
                _factory.CreateBigExplosion(playerShip.Position.Value, playerShip.Direction.Value);
                
                var position = Random.insideUnitCircle * 10;
                var direction = Vector3.forward * Random.Range(0, 360);

                // Reset position after death
                playerShip.Position.Value = position;
                playerShip.Direction.Value = direction;
            }
        }
    }
    
}