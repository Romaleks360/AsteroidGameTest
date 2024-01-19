using System.Linq;
using Common;
using UnityEngine;

namespace Asteroids.Actors {
    
    public static class ActorFactoryExtension {
        
        public static void CreateAsteroids<TAsteroid>(this ActorFactory factory, AsteroidActor parent) where TAsteroid: AsteroidActor {
            var position = parent.Position.Value;
            var direction = parent.Direction.Value;
            
            factory.Create<TAsteroid>(position, Vector3.forward * -90 + direction);
            factory.Create<TAsteroid>(position, Vector3.forward * 90 + direction);

            factory.CreateBigExplosion(position, direction);
        }
        
        public static void CreateBigExplosion(this ActorFactory factory, Vector3 position, Vector3 direction) {
            CreateExplosionWave(factory, position, direction, 9, 12, .2f);
            CreateExplosionWave(factory, position, direction, 8, 10, .3f);
            CreateExplosionWave(factory, position, direction, 6, 8, .5f);
        }
        
        public static void CreateSmallExplosion(this ActorFactory factory, Vector3 position, Vector3 direction) {
            CreateExplosionWave(factory, position, direction, 12, 10, .2f);
            CreateExplosionWave(factory, position, direction, 9, 8, .3f);
        }

        private static void CreateExplosionWave(ActorFactory factory, Vector3 position, Vector3 direction, int count, int speed, float lifeTime) {
            var sector = 360 / count;
            
            Enumerable.Range(1, count).Each(i => {
                var rotation = Vector3.forward * (i * sector);
                var particle = factory.Create<ParticleActor>(position, rotation + direction);
                
                particle.Speed.Value = speed;
                particle.LifeEnd.Value = Time.time + lifeTime;
            });
        }
        
    }
}
