using Asteroids.Actors;
using Common;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Asteroids.Systems {
    public class BoundsSystem : ActorSystem<IMoveableActor>, IActorSystem {

        public void Update() {
            var position = new NativeArray<Vector3>(Actors.Count, Allocator.TempJob);
            var bounds = Camera.main.OrthographicBounds().size;

            Actors.For((i, actor) => {
                position[i] = actor.Position.Value;
            });
            
            new BoundsJob() {
                    Position = position,
                    Bounds = bounds,
                }
                .Schedule(Actors.Count, 8)
                .Complete();
            
            Actors.For((i, actor) => {
                actor.Position.Value = position[i];
            });

            position.Dispose();
        }

        private struct BoundsJob : IJobParallelFor {
            public NativeArray<Vector3> Position;
            
            [ReadOnly]
            public Vector3 Bounds;
            
            public void Execute(int index) {
                var position = Position[index];

                if (position.x > Bounds.x) position = new Vector3(Bounds.x * -1, position.y);
                if (position.x < Bounds.x * -1) position = new Vector3(Bounds.x, position.y);
                
                if (position.y > Bounds.y) position = new Vector3(position.x,Bounds.y * -1 );
                if (position.y < Bounds.y * -1) position = new Vector3(position.x, Bounds.y);
                
                Position[index] = position;
            }
        }
    }
}
